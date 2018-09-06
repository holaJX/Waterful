using System;
using Microsoft.AspNetCore.Mvc;
using Waterful.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace Waterful.Controllers
{
    public class HomeController : Controller
    {
        //private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));

        //MySqlDbContext _context;
        //public HomeController(MySqlDbContext context)
        //{
        //    _context = context;
        //}
        public async Task<IActionResult> Index()
        {
            //_context.Users.Add()
            //log.Error("Controller Error骨灰盒发极光个计划{0}");


            using (HttpClient httpClient = new HttpClient())
            {
                var strm = await httpClient.GetStreamAsync("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQFc8DwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyWFI3ZDB6NHU4UV8xMDAwMDAwN2wAAgQ6f0dZAwQAAAAA");
                using (StreamWriter sw = new StreamWriter(new FileStream(@"C:\temp\123.png", FileMode.Create)))
                {
                    await strm.CopyToAsync(sw.BaseStream);
                    await sw.FlushAsync();
                    
                }
            }
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
