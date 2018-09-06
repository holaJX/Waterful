using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Waterful.Core.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        IQueryable<Order> SearchList(int startPage, int pageSize, out int rowCount, string begin, string end, string mobile, string name, int status, int type);
        Order FindInclude(Expression<Func<Order, bool>> where);
    }
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public IQueryable<Order> SearchList(int startPage, int pageSize, out int rowCount, string begin, string end, string mobile, string name, int status, int type)
        {
            IQueryable<Order> result = _dbContext.Orders.Include(i => i.OrderItems);
            DateTime beginTime;
            DateTime endTime;
            result = result.Where(i => i.Status > -2);
            //售卖订单 1 租用订单 2  续费售卖 5 续费租用 6
            switch (type)
            {
                case 1:
                    result = result.Where(i => i.OrderType == 1 && i.ParentId == 0);
                    break;
                case 2:
                    result = result.Where(i => i.OrderType == 2 && i.ParentId == 0);
                    break;
                case 5:
                    result = result.Where(i => i.OrderType == 1 && i.ParentId != 0);
                    break;
                case 6:
                    result = result.Where(i => i.OrderType == 2 && i.ParentId != 0);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(begin) && !string.IsNullOrWhiteSpace(end) && DateTime.TryParse(begin, out beginTime) && DateTime.TryParse(end, out endTime))
                result = result.Where(i => i.CreateTime > beginTime && i.CreateTime < endTime);
            if (!string.IsNullOrWhiteSpace(mobile))
                result = result.Where(i => i.Mobile == mobile);
            if (!string.IsNullOrWhiteSpace(name))
                result = result.Where(i => i.Name.Contains(name));
            if (status > -10)
                result = result.Where(i => i.Status == status);

            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }
        public Order FindInclude(Expression<Func<Order, bool>> where)
        {
            var result = _dbContext.Orders.Include(i => i.OrderItems).FirstOrDefault(where);

            return result;
        }
    }
}
