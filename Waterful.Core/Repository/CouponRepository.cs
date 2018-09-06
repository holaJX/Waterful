using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.DTO;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Waterful.Core.Repository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        IQueryable<Coupon> SearchList(int startPage, int pageSize, out int rowCount, CouponDto model);

        IEnumerable<int> CouponIds(int pageIndex, int pageSize, Expression<Func<Coupon, bool>> where);

    }
    public class CouponRepository : RepositoryBase<Coupon>, ICouponRepository
    {

        public CouponRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }

        public IEnumerable<int> CouponIds(int pageIndex, int pageSize, Expression<Func<Coupon, bool>> where)
        {
            IQueryable<Coupon> dbSet = _dbContext.Coupons;
            int row = (pageIndex - 1) * pageSize;
            return dbSet.AsNoTracking().Where(where).OrderBy(m => m.Id).Select(m => m.Id).Skip(row).Take(pageSize).ToList();
        }

        public IQueryable<Coupon> SearchList(int startPage, int pageSize, out int rowCount, CouponDto model)
        {
            IQueryable<Coupon> result = _dbContext.Coupons;
            if (model.CouponType > 0)
                result = result.Where(e => e.CouponType == model.CouponType);
            if (model.Type > 0)
                result = result.Where(e => e.Type == model.Type);
            if (!string.IsNullOrWhiteSpace(model.CouponNo))
                result = result.Where(e => e.CouponNo.Equals(model.CouponNo));
            if (!string.IsNullOrWhiteSpace(model.Name))
                result = result.Where(e => e.Name.Contains(model.Name));
            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }
    }
}

