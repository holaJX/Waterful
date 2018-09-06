using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;

namespace Waterful.Core.Repository
{
    public interface IUserchatRepository : IRepository<Userchat>
    {
        IQueryable<Userchat> SearchList(int pageIndex, int pageSize, out int rowCount, string name);

    }
    public class UserchatRepository : RepositoryBase<Userchat>, IUserchatRepository
    {
        public UserchatRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
        public IQueryable<Userchat> SearchList(int startPage, int pageSize, out int rowCount, string name)
        {
            IQueryable<Userchat> result = _dbContext.Userchats;
            result = result.Where(i => i.Status > -1);
            if (!string.IsNullOrWhiteSpace(name))
                result = result.Where(i => i.Name.Contains(name));
            result = result.OrderByDescending(m => m.Id);
            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).AsNoTracking();
        }
    }
}