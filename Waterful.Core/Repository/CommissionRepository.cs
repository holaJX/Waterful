using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Waterful.Core.Repository
{
    public interface ICommissionRepository : IRepository<Commission>
    {
        Task<List<Commission>> PagerAsync(int pageIndex, int pageSize, Expression<Func<Commission, bool>> where);

    }
    public class CommissionRepository : RepositoryBase<Commission>, ICommissionRepository
    {
        public CommissionRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }

        public Task<List<Commission>> PagerAsync(int pageIndex, int pageSize, Expression<Func<Commission, bool>> where)
        {
            int row = (--pageIndex) * pageSize;

            return _dbContext.Commissions
                .Where(where)
                .OrderByDescending(m => m.Id)
                .Skip(row)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        }
    }
}
