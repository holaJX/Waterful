using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Waterful.Core;
using Waterful.Core.Models;
using Waterful.Core.DTO;
using X.PagedList;
using Waterful.Back.ViewModels;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// 分享管理
    /// </summary>
    public class ShareController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ShareController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string mobile, int p = 1, int pageSize = 10)
        {
            ViewData["mobile"] = mobile;
            p = p < 1 ? 1 : p;
            int count = 0;
            var result = _unitOfWork.CustomerRepository.ShareList(p, pageSize, out count, mobile);
            var pageList = new StaticPagedList<ShareDto>(result, p, pageSize, count);
            return View(pageList);
        }
        public ActionResult Angel(string mobile, int p = 1, int pageSize = 10)
        {
            ViewData["mobile"] = mobile;
            p = p < 1 ? 1 : p;
            int count = 0;
            var result = _unitOfWork.CustomerRepository.ShareAngelList(p, pageSize, out count, mobile);
            var pageList = new StaticPagedList<ShareDto>(result, p, pageSize, count);
            return View(pageList);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            var list1 = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(1, 100, m => m.IntroducId == id);
            var list2 = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(1, 100, m => m.IntroducId == id && m.IsPay);
            var list3 = await _unitOfWork.CommissionRepository.PagerAsync(1, 100, m => m.CustomerId == id);


            ViewData["list1"] = list1;
            ViewData["list2"] = list2;
            ViewData["list3"] = list3;
            //var nlist = list3.Select(m => new ShareGetDetailVM
            //{
            //    Amount = m.Amount.ToString("C"),
            //    CreateTime = m.CreateTime.ToString("yyyy年MM月dd日 HH:mm"),
            //    OrderAmount = m.OrderAmount.ToString("C"),
            //    Rate = m.Rate.ToString("P"),
            //    Status = m.Status == 0 ? "未结算" : "已结算"
            //});

            return View();
        }
        public async Task<IActionResult> AngelDetails(int id)
        {
            if (id < 1)
            {
                return NotFound();
            }
            var list1 = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(1, 100, m => m.IntroducId == id);
            var list2 = await _unitOfWork.CustomerRepository.GetChildrenNickNameAsync(1, 100, m => m.IntroducId == id && m.IsPay);
            var list3 = await _unitOfWork.CommissionRepository.PagerAsync(1, 100, m => m.CustomerId == id);


            ViewData["list1"] = list1;
            ViewData["list2"] = list2;
            ViewData["list3"] = list3;
            //var nlist = list3.Select(m => new ShareGetDetailVM
            //{
            //    Amount = m.Amount.ToString("C"),
            //    CreateTime = m.CreateTime.ToString("yyyy年MM月dd日 HH:mm"),
            //    OrderAmount = m.OrderAmount.ToString("C"),
            //    Rate = m.Rate.ToString("P"),
            //    Status = m.Status == 0 ? "未结算" : "已结算"
            //});

            return View();
        }

        /// <summary>
        /// 结算
        /// </summary>
        [HttpPost]
        public JsonResult Set(int id)
        {
            AjaxResult dto = new AjaxResult();
            if (id < 1)
            {
                dto.msg = "参数错误";
                return Json(dto);
            }
            var entity = _unitOfWork.CommissionRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (entity == null)
            {
                dto.msg = "结算记录不存在";
                return Json(dto);
            }
            if (entity.Status==0)
            {
                entity.Status = 1;
                entity.UpdateTime = DateTime.Now;
                _unitOfWork.CommissionRepository.Update(entity);
            }
           
            dto.msg = "修改成功";
            dto.err = 1;
            return Json(dto);
        }
//#if DEBUG
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Share/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

//        // GET: Share/Edit/5
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

//        // POST: Share/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

//        // GET: Share/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var customer = await _context.Customers
//                .SingleOrDefaultAsync(m => m.Id == id);
//            if (customer == null)
//            {
//                return NotFound();
//            }

//            return View(customer);
//        }

//        // POST: Share/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
//            _context.Customers.Remove(customer);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        private bool CustomerExists(int id)
//        {
//            return _context.Customers.Any(e => e.Id == id);
//        }
//#endif

    }
}
