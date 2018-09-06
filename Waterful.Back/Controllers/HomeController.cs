using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Waterful.Back.Controllers
{
    public class HomeController : LoginControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Error()
        {
            return View();
        }
    }
}
