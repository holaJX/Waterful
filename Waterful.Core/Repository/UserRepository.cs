using Waterful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Waterful.Core.Repository
{
    /// <summary>
    /// 用户管理仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        User CheckUser(string userName, string password);
        List<LoginVM> SearchList(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<User, bool>> where = null, System.Linq.Expressions.Expression<Func<User, object>> order = null);

        //User GetWithRoles(int id);
    }
    public class LoginVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    /// <summary>
    /// 用户管理仓储实现
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
       
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(it => it.UserName == userName && it.Password == password && it.Status > 0);
        }
        public List<LoginVM> SearchList(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<User, bool>> where = null, System.Linq.Expressions.Expression<Func<User, object>> order = null)
        {
            int pageStart = (pageIndex - 1) * pageSize;
            int pageEnd = pageSize;
            var sdfj = base.ExecuteReader<LoginVM>("select * from users limit @pageStart,@pageEnd;",
               new MySqlParameter() { ParameterName = "@pageStart", Value = (pageIndex - 1) * pageSize },
               new MySqlParameter() { ParameterName = "@pageEnd", Value = pageSize }

            );
            return sdfj;
        }
        //public List<User> GetUserList(int id, int pageIndex, int pageSize, out int rowCount)
        //{
        //    return _dbContext.Set<User>().(it => it.UserName == userName && it.Password == password);
        //}
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        //public User GetWithRoles(int id)
        //{
        //    var user = _dbContext.Set<User>().FirstOrDefault(it => it.Id == id);
        //    //if (user != null)
        //    //{
        //    //    user.UserRoles = _dbContext.Set<UserRole>().Where(it => it.UserId == id).ToList();
        //    //}
        //    return user;
        //}
    }
}
