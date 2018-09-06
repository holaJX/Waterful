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
    public class CommentController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CommentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// ∆¿º€¡–±Ì
        /// </summary>
        /// <param name="name"></param>
        /// <param name="p"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string begin, string end, int p = 1, int pageSize = 10)
        {
            p = p < 1 ? 1 : p;
            int count = 0;
            IQueryable<Aftersale> result;
            DateTime beginTime;
            DateTime endTime;
            if (!string.IsNullOrWhiteSpace(begin) && !string.IsNullOrWhiteSpace(end) && DateTime.TryParse(begin, out beginTime) && DateTime.TryParse(end, out endTime))
            {
                ViewData["begin"] = begin;
                ViewData["end"] = end;
                result = _unitOfWork.AftersaleRepository.SearchList(p, pageSize, out count, i => i.Status > 0 && i.CreateTime > beginTime && i.CreateTime < endTime);
            }
            else
            {
                result = _unitOfWork.AftersaleRepository.SearchList(p, pageSize, out count, i => i.Status > 0);
            }
            var s = new StaticPagedList<Aftersale>(result, p, pageSize, count);
            return View(s);
        }
    }
}
