using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Waterful.Core.Repository
{
    public interface IUserinfoRepository : IRepository<Userinfo>
    {

    }
    public class UserinfoRepository : RepositoryBase<Userinfo>, IUserinfoRepository
    {

        public UserinfoRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}