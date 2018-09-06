using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waterful.Wechat.ViewModels;
using Waterful.Wechat.Extensions;
using Waterful.Core;
using Waterful.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Waterful.Core.Models;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        public ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View(new ProductDetailsVM(new Product()));
            }
            try
            {
                var product = _unitOfWork.ProductRepository.SingleOrDefault(x => x.Id == id && x.Status == 1);
                if (product == null)
                {
                    return View(new ProductDetailsVM(new Product()));
                }

                var vm = new ProductDetailsVM(product);

                ViewData["NoLogin"] = false;
                if (!User.Identity.IsAuthenticated)
                {
                    ViewData["NoLogin"] = true;
                    ViewData["LoginUrl"] = Url.Action(nameof(PassportController.Auth), "Passport", new { returnUrl = Url.Action("PayDetails", "Order") });
                }
                else
                {
                    if (!User.Identities.PhoneExist())
                    {
                        ViewData["NoLogin"] = true;
                        ViewData["LoginUrl"] = Url.Action(nameof(PassportController.Login), "Passport", new { returnUrl = Url.Action("PayDetails", "Order") });
                    }
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(Details), ex);
                return View(new ProductDetailsVM(new Product()));
            }
        }

        /// <summary>
        /// 服务介绍
        /// </summary>
        /// <returns></returns>
        public IActionResult Introduction(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var product = _unitOfWork.ProductRepository.SingleOrDefault(x => x.Id == id && x.Status == 1);
                if (product == null)
                {
                    return NotFound();
                }

                var vm = new ProductDetailsVM(product);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(Introduction), ex);
                return NotFound();
            }
        }

        /// <summary>
        /// 获取服务规格
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetServices(int? categoryId, int? paymentType)
        {
            var result = new AjaxResult<List<ProductServiceVM>>();

            if (categoryId == null || paymentType == null)
            {
                return null;
            }

            try
            {
                var services = _unitOfWork.ProductRepository
                                   .Find(x => x.CategoryId == (CategoryEnum)categoryId.Value && x.PaymentType == (PaymentEnum)paymentType.Value && x.Status == 1)
                                   .OrderBy(o => o.Level)
                                   .ToList();

                if (services == null)
                {
                    return null;
                }

                var data = new List<ProductServiceVM>();

                services.ForEach(i =>
                {
                    data.Add(new ProductServiceVM()
                    {
                        FilterPrice = i.FilterPrice,
                        Id = i.Id,
                        Level = i.Level,
                        OriginalPrice = i.OriginalPrice,
                        Price = i.Price,
                        Name = i.Name,
                        Summary = i.Summary
                    });
                });

                result.err = 1;
                result.data = data;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(GetServices), ex);
                return null;
            }

            return Ok(result);
        }
    }
}