using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Waterful.Core.Models;
using Waterful.Core;
using Waterful.Core.Enums;
using Waterful.Back.ViewModels;
using System.Linq.Expressions;
using Waterful.Core.DTO;
using System.Threading;

namespace Waterful.Back.Controllers
{
    public class CouponController : LoginControllerBase
    {
        private IdGenerationService _IdGenerationService;
        private readonly UnitOfWork _unitOfWork;
        public CouponController(IdGenerationService idGenerationService, UnitOfWork unitOfWork)
        {
            _IdGenerationService = idGenerationService;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 加载列表/查询
        /// </summary>
        /// <param name="couponType"></param>
        /// <param name="type"></param>
        /// <param name="p"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult Index(int coupontype, int type, string name, string couponno, int p = 1, int pageSize = 10)
        {
            ViewData["name"] = name;
            ViewData["type"] = type;
            ViewData["coupontype"] = coupontype;
            ViewData["couponno"] = couponno;
            int rowCount;
            p = p < 1 ? 1 : p;
            CouponDto dto = new CouponDto() { CouponType = (CouponEnum)coupontype, Type = (CouponTypeEnum)type, Name = name, CouponNo = couponno };
            var result = _unitOfWork.CouponRepository.SearchList(p, pageSize, out rowCount, dto);
            var res = new X.PagedList.StaticPagedList<Coupon>(result, p, pageSize, rowCount);
            return View(res);
        }

        public IActionResult Create()
        {
            CouponVM vm = new CouponVM();
            return View(vm);
        }
        /// <summary>
        /// 批量生成优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(CouponVM coupon)
        {
            if (coupon != null)
            {
                if (coupon.Type > 0 && coupon.ExpiryNum > 0 && coupon.Number > 0 && coupon.Number <= 500)
                {
                    List<Coupon> list = new List<Coupon>();
                    DateTime dt = DateTime.Now;
                    Coupon model;
                    Queue<string> queue = new Queue<string>();
                    string str = string.Empty;

                    for (int i = 0; i < coupon.Number; i++)
                    {
                        str = CreateCouponNo();
                        while (queue.Contains(str))
                        {
                            str = CreateCouponNo();
                        }
                        queue.Enqueue(str);
                    }

                    for (int i = 0; i < coupon.Number; i++)
                    {
                        model = new Coupon();
                        //兑换码：唯一
                        model.Name = coupon.Name;
                        model.CouponNo = queue.Dequeue();//_IdGenerationService.GenerateId().ToString();
                        model.CouponType = coupon.CouponType;
                        model.Type = coupon.Type;
                        model.CreateTime = dt;
                        model.UpdateTime = dt;
                        model.ExpiryDate = dt.AddMonths(coupon.ExpiryNum);
                        model.Discount = coupon.Discount;
                        model.FeelTime = coupon.FeelTime;
                        model.Remark = coupon.Remark;
                        model.Status = 1;
                        list.Add(model);
                    }
                    _unitOfWork.CouponRepository.Insert(list);
                    _unitOfWork.SaveChange();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorInfo = "参数不符合规范，请检查后再提交";
                }
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = _unitOfWork.CouponRepository.FirstOrDefault(e => e.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            ViewData["type"] = (int)coupon.Type;
            ViewData["coupontype"] = (int)coupon.CouponType;
            return View(coupon);
        }
        [HttpPost]
        public IActionResult Edit(int id, Coupon coupon)
        {
            Coupon model = null;
            if (id > 0)
            {
                model = _unitOfWork.CouponRepository.FirstOrDefault(e => e.Id == id);
                if (model == null)
                    return NotFound();
            }
            try
            {
                if (model.Used)
                {
                    ViewBag.ErrorInfo = "券已使用";
                    return View(coupon);
                }
                if (coupon.Type > 0 && coupon.CouponType > 0 && !string.IsNullOrWhiteSpace(coupon.Name))
                {
                    model.Name = coupon.Name;
                    model.CouponType = coupon.CouponType;
                    model.Type = coupon.Type;
                    model.UpdateTime = DateTime.Now;
                    model.Discount = coupon.Discount;
                    model.FeelTime = coupon.FeelTime;
                    model.Remark = coupon.Remark;
                    _unitOfWork.CouponRepository.Update(model);
                    _unitOfWork.SaveChange();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorInfo = "参数不符合规范，请检查后再提交";
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorInfo = ex.Message;
            }

            return View(model);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.CouponRepository.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var coupon = _unitOfWork.CouponRepository.FirstOrDefault(m => m.Id == id);
            _unitOfWork.CouponRepository.Delete(coupon);
            return RedirectToAction("Index");
        }

        static Random rnd = new Random();
        static int seed = 0;
        public static string CreateCouponNo()
        {
            var rndData = new byte[4];
            rnd.NextBytes(rndData);

            var seedValue = Interlocked.Add(ref seed, 1);
            var seedData = BitConverter.GetBytes(seedValue);
            var tokenData = rndData.Concat(seedData).OrderBy(_ => rnd.Next());
            string encode = Convert.ToBase64String(tokenData.ToArray()).TrimEnd('=').Replace("/", "").Replace("+", "").Trim();
            return encode.ToUpper().Replace("I", "").Replace("O", "").Substring(0, 6);
        }

    }
}