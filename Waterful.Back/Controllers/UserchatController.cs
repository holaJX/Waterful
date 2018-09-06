using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Waterful.Core;
using Waterful.Core.Models;
using X.PagedList;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// 聊天记录管理
    /// </summary>
    public class UserchatController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public UserchatController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string name, int p = 1, int pageSize = 10)
        {
            ViewData["Search"] = name;
            p = p < 1 ? 1 : p;
            int count = 0;
            var result = _unitOfWork.UserchatRepository.SearchList(p, pageSize, out count, name);
            var s = new StaticPagedList<Userchat>(result, p, pageSize, count);
            return View(s);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        public IActionResult Create()
        {
            var m = new Userchat();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,Content,Remark,Id")] Userchat model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ViewBag.ErrorInfo = "姓名必填。";
                    return View(model);
                }
                _unitOfWork.UserchatRepository.Insert(model);
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

        public ActionResult Edit(int id)
        {
            var model = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Name,Content,Remark,Id")] Userchat model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ViewBag.ErrorInfo = "姓名必填。";
                    return View(model);
                }
               
                var entity = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Content = model.Content;
                entity.Remark = model.Remark;
                entity.UpdateTime = DateTime.Now;
                _unitOfWork.UserchatRepository.Update(entity);

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
        public ActionResult Edit1(int id)
        {
            var model = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1(int id, [Bind("Name,Content,Remark,Id")] Userchat model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ViewBag.ErrorInfo = "姓名必填。";
                    return View(model);
                }

                var entity = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Content = model.Content;
                entity.Remark = model.Remark;
                entity.UpdateTime = DateTime.Now;
                _unitOfWork.UserchatRepository.Update(entity);

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
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var entity = _unitOfWork.UserchatRepository.FirstOrDefault(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Status = -1;
            entity.UpdateTime = DateTime.Now;
            _unitOfWork.UserchatRepository.Update(entity);

            return RedirectToAction("Index");
        }
    }
}
