using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Waterful.Core.Repository;
using X.PagedList;
using Waterful.Core.Models;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Waterful.Core;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class CustomerController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string begin, string end, string mobile, string name, int isAngel = 0, int p = 1, int pageSize = 10)
        {
            p = p < 1 ? 1 : p;
            int count = 0;
            IQueryable<Customer> result;
            //DateTime beginTime;
            //DateTime endTime;

            if (!string.IsNullOrWhiteSpace(begin) && !string.IsNullOrWhiteSpace(end))
            {
                ViewData["begin"] = begin;
                ViewData["end"] = end;
            }
            ViewData["mobile"] = mobile;
            ViewData["name"] = name;
            ViewData["isAngel"] = isAngel;

            result = _unitOfWork.CustomerRepository.SearchList(p, pageSize, out count, begin, end, mobile, name, isAngel);

            var pageList = new StaticPagedList<Customer>(result, p, pageSize, count);

            return View(pageList);
        }

        public ActionResult Details(int id = 0)
        {
            if (id < 1)
            {
                return NotFound();
            }

            var customer = _unitOfWork.CustomerRepository.FindInclude(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        #region 设置大使用户

        public ActionResult Set(int id)
        {
            var entity = _unitOfWork.CustomerRepository.FirstOrDefault(m => m.Id == id && m.Status > 0);

            return View(entity);
        }

        [HttpPost, ActionName("Set")]
        [ValidateAntiForgeryToken]
        public ActionResult SetConfirmed(int id, int type)
        {
            var entity = _unitOfWork.CustomerRepository.FirstOrDefault(m => m.Id == id && m.Status > 0);
            if (entity == null)
            {
                return NotFound();
            }
            entity.IsAngel = (type == 1 ? true : false);
            entity.UpdateTime = DateTime.Now;
            _unitOfWork.CustomerRepository.Update(entity);

            return RedirectToAction("Index");
        }
        #endregion

        #region 用户画像

        /// <summary>
        /// 用户画像(有则更新无则插入)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            var entity = _unitOfWork.UserinfoRepository.FirstOrDefault(m => m.CustomerId == id && m.Status > -1);
            if (entity == null)
            {
                entity = new Userinfo();
            }
            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Info([Bind("Name,Content,Remark,Id")] Userinfo model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Content))
                {
                    ViewBag.ErrorInfo = "内容必须。";
                    return View(model);
                }
                var entity = _unitOfWork.UserinfoRepository.FirstOrDefault(m => m.CustomerId == model.Id && m.Status > -1);
                if (entity == null)
                {
                    entity = new Userinfo();
                    entity.CustomerId = model.Id;

                    entity.Name = model.Name;
                    entity.Content = model.Content;
                    entity.Remark = model.Remark;
                    _unitOfWork.UserinfoRepository.Insert(entity);
                }
                else
                {
                    entity.Name = model.Name;
                    entity.Content = model.Content;
                    entity.Remark = model.Remark;
                    entity.UpdateTime = DateTime.Now;
                    _unitOfWork.UserinfoRepository.Update(entity);
                }
                return RedirectToAction("Index");
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
        #endregion

        //#if DEBUG
        //        public IActionResult Create()
        //        {
        //            var m = new Customer();
        //            return View(m);
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Create([Bind("OpenId,NickName,Mobile,FullName,Ico,IntroducId,Status,RegisterType,CustomerType,Remark,CreateTime,UpdateTime,Id")] Customer customer)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _context.Add(customer);
        //                await _context.SaveChangesAsync();
        //                return RedirectToAction("Index");
        //            }
        //            return View(customer);
        //        }

        //        // GET: CustomerDelete/Edit/5
        //        public async Task<IActionResult> Edit(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
        //            if (customer == null)
        //            {
        //                return NotFound();
        //            }
        //            return View(customer);
        //        }


        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Edit(int id, [Bind("OpenId,NickName,Mobile,FullName,Ico,IntroducId,Status,RegisterType,CustomerType,Remark,CreateTime,UpdateTime,Id")] Customer customer)
        //        {
        //            if (id != customer.Id)
        //            {
        //                return NotFound();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _context.Update(customer);
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!CustomerExists(customer.Id))
        //                    {
        //                        return NotFound();
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction("Index");
        //            }
        //            return View(customer);
        //        }
        //        private bool CustomerExists(int id)
        //        {
        //            return _context.Customers.Any(e => e.Id == id);
        //        }
        //#endif
    }
}