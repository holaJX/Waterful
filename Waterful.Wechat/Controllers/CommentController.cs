using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waterful.Core;
using Waterful.Wechat.ViewModels;
using Waterful.Core.Enums;
using Waterful.Wechat.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Waterful.Wechat.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        public CommentController(ILogger<CommentController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <param name="aid">服务id</param>
        /// <returns></returns>
        public IActionResult Index(int aid, int oid)
        {
            CommentVM vm = new CommentVM();
            vm.Id = aid;
            vm.OId = oid;

            var aftersale = _unitOfWork.AftersaleRepository.FirstOrDefault(i => i.Id == aid && i.OrderId == oid && i.Status == 1);
            if (aftersale == null || aftersale.CustomerId != User.Identities.DefaultCustomerId())
            {
                //该订单已服务过或不存在
                return Content("该订单已服务过或不存在");
            }
            var worker = _unitOfWork.WorkerRepository.FirstOrDefault(i => i.Id == aftersale.WorkerId);
            vm.Logo = worker.Logo;
            vm.Name = worker.Name;
            vm.ServiceTime = worker.UpdateTime.ToString();
            return View(vm);
        }
        [HttpPost]
        public JsonResult Send(CommentVM vm)//int oid, int aid,string star, string isOnTime)
        {
            AjaxResult dto = new AjaxResult();
            //判断服务记录存在
            var aftersale = _unitOfWork.AftersaleRepository.FirstOrDefault(i => i.Id == vm.Id);

            if (User.Identities == null)
            {
                dto.msg = $"您没有登录";
                return Json(dto);
            }
            if (aftersale == null)
            {
                dto.msg = "该订单已服务过或服务不存在";
                return Json(dto);
            }
            if (aftersale.CustomerId != User.Identities.DefaultCustomerId())
            {
                dto.msg = "订单和当前用户不匹配";
                return Json(dto);
            }

            //服务更改状态
            aftersale.Status = 2;

            aftersale.Grade = vm.star;
            aftersale.Content = vm.Content;
            aftersale.isOnTime = vm.isOnTime == 0 ? false : true;
            aftersale.isTidy = vm.isTidy == 0 ? false : true;
            aftersale.isClear = vm.isClear == 0 ? false : true;
            aftersale.Content = vm.Content;

            _unitOfWork.AftersaleRepository.Update(aftersale);
            dto.err = 1;
            dto.msg = $"/Order/Index";

            return Json(dto);
        }

        /// <summary>
        /// 确认服务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Confirm(int id)
        {
            AjaxResult dto = new AjaxResult();

            try
            {
                var status = 0;
                var aftersale = _unitOfWork.AftersaleRepository
                                    .Find(x => x.Id == id && x.Status == status)
                                    .Include(x => x.Order)
                                    .SingleOrDefault();

                if (aftersale == null)
                {
                    dto.msg = "该服务已确认或不存在。";
                }
                else
                {
                    if (aftersale.Order == null)
                    {
                        dto.msg = "订单信息不存在。";
                    }
                    else
                    {
                        aftersale.Status = 1;
                        aftersale.UpdateTime = DateTime.Now;
                        aftersale.Order.ServiceNumber--;

                        _unitOfWork.AftersaleRepository.Update(aftersale, false);
                        _unitOfWork.OrderRepository.Update(aftersale.Order, false);
                        _unitOfWork.SaveChange();

                        dto.err = 1;
                        dto.msg = "服务确认成功。";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Confirm), ex);
            }
            return Ok(dto);
        }
    }
}
