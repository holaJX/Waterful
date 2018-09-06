using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waterful.Core;
using Waterful.Back.ViewModels;
using Waterful.Core.Models;
using Waterful.Core.Repository;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public class InitController : LoginControllerBase
    {
#if DEBUG
        private readonly PomeloMySqlDbContext _context;

        public InitController(PomeloMySqlDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var asdf = DbInitializer.Initialize(_context);
            return Content(asdf.ToString());
        }
#endif
    }
}