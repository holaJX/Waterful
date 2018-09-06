using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Waterful.Models;
using System.Data.SqlClient;
using System.Reflection;

namespace Waterful.Controllers
{
    public class UsersController : Controller
    {
        private readonly MySqlDbContext _context;

        public UsersController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        public IActionResult Index1()
        {
            var dfd = _context.Set<User123>().FromSql("select name,lastname as name1 from users");

            return Content(dfd.FirstOrDefault().Name1);
        }
        public static IList<T> SqlQuery<T>(DbContext db, string sql, params object[] parameters)
            where T : new()
        {
            //注意：不要对GetDbConnection获取到的conn进行using或者调用Dispose，否则DbContext后续不能再进行使用了，会抛异常
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    var propts = typeof(T).GetProperties();
                    var rtnList = new List<T>();
                    T model;
                    object val;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new T();
                            foreach (var l in propts)
                            {
                                val = reader[l.Name];
                                if (val == DBNull.Value)
                                {
                                    l.SetValue(model, null);
                                }
                                else
                                {
                                    l.SetValue(model, val);
                                }
                            }
                            rtnList.Add(model);
                        }
                    }
                    return rtnList;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public IActionResult ShiYong()
        {
            //var db = _context;
            //string name = "tom";
            //var list = SqlQuery<PAModel>(db,
            //    $" select p.id, p.name, a.fullAddress, a.lat, a.lon " +
            //    $" from ( select * from {nameof(Person)} where {nameof(name)}=@{nameof(name)} ) as p " +
            //    $" left join {nameof(Address)} as a on p.addrid = a.id ",
            //    new[] { new SqlParameter(nameof(name), name) });
            return Content("");
        }
        public IActionResult Index2()
        {
            //var sdfklj =  _context.Database.ExecuteSqlCommand("");
            var list = new List<User>();
            var conn = _context.Database.GetDbConnection();
            conn.Open();
            try
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "select * from users";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var blog = new User();
                            blog.Id = Convert.ToInt32(reader["Id"]);
                            blog.Name = reader["Name"].ToString();
                            //blog.Url = reader["Url"].ToString();
                            list.Add(blog);
                        }
                    }
                }
            }
            finally
            {
                conn.Close();
            }
            return Content(list.FirstOrDefault().Name);
            //using (var connection = _context.Database.GetDbConnection())
            //{
            //    connection.Open();

            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = "select * from [users]";
            //        using (SqlDataReader reader = command.ExecuteReader() as SqlDataReader)
            //        {
            //            while (reader.Read())
            //            {
            //                var blog = new User();
            //                blog.Id = Convert.ToInt32(reader["Id"]);
            //                blog.Name = reader["Name"].ToString();
            //                //blog.Url = reader["Url"].ToString();
            //                list.Add(blog);
            //            }
            //        }
            //    }
            //}
            //return Content(list.FirstOrDefault().Name);
        }
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,LastName")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,LastName")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
