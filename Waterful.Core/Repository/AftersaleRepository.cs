using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Waterful.Core.Repository
{
    public interface IAftersaleRepository : IRepository<Aftersale>
    {
        IQueryable<Aftersale> SearchList(int startPage, int pageSize, out int rowCount, Expression<Func<Aftersale, bool>> where = null, Expression<Func<Aftersale, object>> order = null);
        //List<Aftersale> GetList(Expression<Func<Aftersale, bool>> predicate);

    }
    public class AftersaleRepository : RepositoryBase<Aftersale>, IAftersaleRepository
    {

        public AftersaleRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public IQueryable<Aftersale> SearchList(int startPage, int pageSize, out int rowCount, Expression<Func<Aftersale, bool>> where = null, Expression<Func<Aftersale, object>> order = null)
        {
            IQueryable<Aftersale> result = _dbContext.Aftersales.Include(i => i.Worker);

            if (where != null)
                result = result.Where(where);

            result = order != null ? result.OrderByDescending(order) : result.OrderByDescending(m => m.Id);
            
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }
        //public List<Aftersale> GetList(Expression<Func<Aftersale, bool>> predicate)
        //{
        //    return _dbContext.Set<TEntity>().ToList();
        //}

    }
}

