using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Waterful.Core.DTO;

namespace Waterful.Core.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// 获取用户的分享二维码
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        Task<CustomerQrImgDto> GetQrImgAsync(int cid);
        /// <summary>
        /// 获取推荐的会员昵称
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<List<string>> GetChildrenNickNameAsync(int pageIndex, int pageSize, Expression<Func<Customer, bool>> where);
        //Task<List<string>> GetChildrenNickName(int pageIndex, int pageSize, int customerId);
        IQueryable<Customer> SearchList(int startPage, int pageSize, out int rowCount, string begin, string end, string mobile, string name, int isAngel);
        Customer FindInclude(int id);
        IQueryable<ShareDto> ShareList(int startPage, int pageSize, out int rowCount, string mobile);
        IQueryable<ShareDto> ShareAngelList(int startPage, int pageSize, out int rowCount, string mobile);

        CustomerPayDto CustomerPayInfo(int customerId);
    }
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public Customer FindInclude(int id)
        {
            var result = _dbContext.Customers.Include(i => i.Addresses).FirstOrDefault(i => i.Id == id && i.Status > 0);

            return result;
        }

        public IQueryable<Customer> SearchList(int startPage, int pageSize, out int rowCount, string begin, string end, string mobile, string name, int isAngel)
        {
            IQueryable<Customer> result = _dbContext.Customers;
            DateTime beginTime;
            DateTime endTime;
            result = result.Where(i => i.Status > 0);
            if (!string.IsNullOrWhiteSpace(begin) && !string.IsNullOrWhiteSpace(end) && DateTime.TryParse(begin, out beginTime) && DateTime.TryParse(end, out endTime))
                result = result.Where(i => i.CreateTime > beginTime && i.CreateTime < endTime);
            if (!string.IsNullOrWhiteSpace(mobile))
                result = result.Where(i => i.Mobile == mobile);
            if (!string.IsNullOrWhiteSpace(name))
                result = result.Where(i => i.NickName.Contains(name));
            if (isAngel == 1)//普通
            {
                result = result.Where(i => !i.IsAngel && !i.IsPay);
            }
            else if (isAngel == 2)//天使
            {
                result = result.Where(i => !i.IsAngel && i.IsPay);
            }
            else if (isAngel == 3)//大使
            {
                result = result.Where(i => i.IsAngel);
            }
            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public Task<CustomerQrImgDto> GetQrImgAsync(int cid)
        {
            return _dbContext.Customers
                .Where(m => m.Id == cid)
                .Select(m => new CustomerQrImgDto
                {
                    CustomerId = m.Id,
                    NickName = m.NickName,
                    QrImgUrl = m.QrImg
                })
                .AsNoTracking()
                .SingleAsync();
        }

        public Task<List<string>> GetChildrenNickNameAsync(int pageIndex, int pageSize, Expression<Func<Customer, bool>> where)
        {
            int row = (--pageIndex) * pageSize;

            return _dbContext.Customers
                .Where(where)
                .OrderByDescending(m => m.Id)
                .Select(m => m.NickName)
                .Skip(row)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        }
        //        public Task<List<string>> GetChildrenNickName(int pageIndex, int pageSize, int customerId)
        //        {
        //            int row = (--pageIndex) * pageSize;

        //            return _dbContext.Customers.FromSql($@"
        //SELECT c.NickName
        //FROM orders o
        // LEFT JOIN customers c ON o.CustomerId = c.Id
        //WHERE o.Status = 1 AND c.IntroducId = {customerId.ToString()}
        //GROUP BY c.Id
        //ORDER BY c.Id LIMIT {(pageIndex * pageSize).ToString()},{pageSize.ToString()}
        //                ")
        //                .Select(m => m.NickName)
        //                .ToListAsync();

        //        }


        /// <summary>
        /// 后台分享统表(非大使用户)
        /// </summary>
        public IQueryable<ShareDto> ShareList(int startPage, int pageSize, out int rowCount, string mobile)
        {
            Expression<Func<Customer, bool>> where;
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                where = i => i.Status > 0 && !i.IsAngel && i.Mobile == mobile;
            }
            else
            {
                where = i => i.Status > 0 && !i.IsAngel;
            }
            var group = _dbContext.Customers.Where(i => i.Status > 0).GroupBy(i => i.IntroducId).Select(i => new
            {
                IntroducId = i.Key,
                Count = i.Count(),
                PayCount = i.Count(j => j.IsPay)
            });
            var result = group.Join(_dbContext.Customers.Where(where),
                g => g.IntroducId, c => c.Id, (i, c) => new ShareDto
                {
                    Id = c.Id,
                    Mobile = c.Mobile,
                    FullName = c.FullName,
                    NickName = c.NickName,
                    Count = i.Count,
                    PayCount = i.PayCount
                });
            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }
        /// <summary>
        /// 后台分享统表(大使用户)
        /// </summary>
        public IQueryable<ShareDto> ShareAngelList(int startPage, int pageSize, out int rowCount, string mobile)
        {
            Expression<Func<Customer, bool>> where;
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                where = i => i.Status > 0 && i.IsAngel && i.Mobile == mobile;
            }
            else
            {
                where = i => i.Status > 0 && i.IsAngel;
            }
            var group = _dbContext.Customers.Where(i => i.Status > 0).GroupBy(i => i.IntroducId).Select(i => new
            {
                IntroducId = i.Key,
                Count = i.Count(),
                PayCount = i.Count(j => j.IsPay)
            });
            var result = group.Join(_dbContext.Customers.Where(where),
                g => g.IntroducId, c => c.Id, (i, c) => new ShareDto
                {
                    Id = c.Id,
                    Mobile = c.Mobile,
                    FullName = c.FullName,
                    NickName = c.NickName,
                    Count = i.Count,
                    PayCount = i.PayCount
                });
            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public CustomerPayDto CustomerPayInfo(int customerId)
        {
            return _dbContext.Customers
                  .Select(m => new CustomerPayDto
                  {
                      IntroducId = m.IntroducId,
                      IsAngel = m.IsAngel,
                      IsPay = m.IsPay
                  })
                  .FromSql($@"
                        SELECT c.IsPay,c.IntroducId,CONVERT(IFNULL(c1.IsAngel,0),SIGNED) AS IsAngel
                        FROM {nameof(Customer)}s c
                          LEFT JOIN {nameof(Customer)}s c1 ON c.IntroducId=c1.Id
                        WHERE c.Id={customerId}
                    ")
                  .AsNoTracking()
                  .SingleOrDefault();
        }
    }
}