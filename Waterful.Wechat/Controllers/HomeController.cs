using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waterful.Core;
using Waterful.Wechat.ViewModels;
using Waterful.Core.Enums;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Concurrent;
using Waterful.Wechat.Extensions;
using System.Threading;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 微商城-首页（产品列表）
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                var productlist = _unitOfWork.ProductRepository.GetWechatProductList();

                var model = new ProductListVM()
                {
                    ClearBuyId = productlist.Where(x => x.CategoryId == CategoryEnum.ClearWater && x.PaymentType == PaymentEnum.Buy).Select(s => s.Id).FirstOrDefault(),
                    ClearRentId = productlist.Where(x => x.CategoryId == CategoryEnum.ClearWater && x.PaymentType == PaymentEnum.Rent).Select(s => s.Id).FirstOrDefault(),
                    DrinkId = productlist.Where(x => x.CategoryId == CategoryEnum.DrinkWater).Select(s => s.Id).FirstOrDefault(),
                    ShowerId = productlist.Where(x => x.CategoryId == CategoryEnum.Shower).Select(s => s.Id).FirstOrDefault()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("{0}-{1}", nameof(Index), ex);
            }

            return Content("系统异常,请稍后尝试。");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Fillter()
        {
            return View();
        }


        public IActionResult Sell()
        {
            return View();
        }
        public IActionResult Rent()
        {
            return View();
        }
    }
}
