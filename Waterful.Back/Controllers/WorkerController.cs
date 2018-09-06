using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Waterful.Core;
using Waterful.Core.Models;
using Waterful.Core.Repository;
using X.PagedList;

namespace Waterful.Back.Controllers
{
    public class WorkerController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public WorkerController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 服务人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string name, int p = 1, int pageSize = 10)
        {
            ViewData["Search"] = name;
            p = p < 1 ? 1 : p;
            var result = _unitOfWork.WorkerRepository.SearchList(p, pageSize, name);
            var count = _unitOfWork.WorkerRepository.SearchTotal(name);
            var s = new StaticPagedList<Worker>(result, p, pageSize, count);
            return View(s);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var worker = _unitOfWork.WorkerRepository
                .FirstOrDefault(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        public IActionResult Create()
        {
            var m = new Worker();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("WorkerNo,Name,Mobile,Logo,Remark,Id")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(worker.WorkerNo) || string.IsNullOrWhiteSpace(worker.Name))
                {
                    ViewBag.ErrorInfo = "工号和姓名必须。";
                    return View(worker);
                }
                if (worker.WorkerNo.Length > 20 || worker.Name.Length > 20)
                {
                    ViewBag.ErrorInfo = "工号和姓名最大长度为20。";
                    return View(worker);
                }
                if (worker.Mobile != null && worker.Mobile.Length > 20)
                {
                    ViewBag.ErrorInfo = "电话长度过长。";
                    return View(worker);
                }
                var b = _unitOfWork.WorkerRepository.Any(i => i.WorkerNo == worker.WorkerNo && i.Status > -1);
                if (b)
                {
                    ViewBag.ErrorInfo = "工号已存在。";
                    return View(worker);
                }
                _unitOfWork.WorkerRepository.Insert(worker);
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
            return View(worker);
        }

        public ActionResult Edit(int id)
        {
            var worker = _unitOfWork.WorkerRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        // POST: Worker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("WorkerNo,Name,Mobile,Logo,Remark,Id")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(worker.WorkerNo) || string.IsNullOrWhiteSpace(worker.Name))
                {
                    ViewBag.ErrorInfo = "工号和姓名必须。";
                    return View(worker);
                }
                if (worker.WorkerNo.Length > 20 || worker.Name.Length > 20)
                {
                    ViewBag.ErrorInfo = "工号和姓名最大长度为20。";
                    return View(worker);
                }
                if (worker.Mobile != null && worker.Mobile.Length > 20)
                {
                    ViewBag.ErrorInfo = "电话长度过长。";
                    return View(worker);
                }
                var b = _unitOfWork.WorkerRepository.Any(i => i.WorkerNo == worker.WorkerNo && i.Id != worker.Id && i.Status > -1);
                if (b)
                {
                    ViewBag.ErrorInfo = "工号已存在。";
                    return View(worker);
                }

                var entity = _unitOfWork.WorkerRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.WorkerNo = worker.WorkerNo;
                entity.Name = worker.Name;
                entity.Logo = worker.Logo;
                entity.Mobile = worker.Mobile;
                entity.Remark = worker.Remark;
                entity.UpdateTime = DateTime.Now;
                _unitOfWork.WorkerRepository.Update(entity);

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
            return View(worker);
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var entity = _unitOfWork.WorkerRepository.FirstOrDefault(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Status = -1;
            entity.UpdateTime = DateTime.Now;
            _unitOfWork.WorkerRepository.Update(entity);

            return RedirectToAction("Index");
        }
    }
}
