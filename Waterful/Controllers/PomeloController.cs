using System;
using Microsoft.AspNetCore.Mvc;
using Waterful.Models;

namespace Waterful.Controllers
{
    public class PomeloController : Controller
    {
        //private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));

        PomeloMySqlDbContext _context;
        public PomeloController(PomeloMySqlDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //_context.Users.Add()
            //log.Error("Controller Error骨灰盒发极光个计划{0}");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
