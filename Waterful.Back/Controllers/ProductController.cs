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
using Waterful.Core.Enums;
using System.Linq.Expressions;
using Npoi.Core.SS.UserModel;
using Waterful.Back.Export;
using System.IO;
using Npoi.Core.XWPF.UserModel;
using Microsoft.AspNetCore.Hosting;

namespace Waterful.Back.Controllers
{
    public class ProductController : LoginControllerBase
    {
        private IHostingEnvironment _host = null;
        private readonly UnitOfWork _unitOfWork;
        public ProductController(UnitOfWork unitOfWork, IHostingEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _host = host;
        }

        // GET: Product
        public IActionResult Index(int categoryId, int p = 1, int pageSize = 10)
        {
            int rowCount;
            p = p < 1 ? 1 : p;
            IQueryable<Product> result;
            ViewData["categoryId"] = categoryId;
            Expression<Func<Product, bool>> expression = e => e.PaymentType == PaymentEnum.Buy && e.Status > -1;
            if (categoryId > 0)
            {
                expression = e => e.CategoryId == (CategoryEnum)categoryId && e.PaymentType == PaymentEnum.Buy && e.Status > -1;
            }
            result = _unitOfWork.ProductRepository.LoadPageList(p, pageSize, out rowCount, expression, e => e.Id);
            var res = new StaticPagedList<Product>(result, p, pageSize, rowCount);
            return View(res);
        }

        // GET: Product/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.FirstOrDefault(m => m.Id == id && m.Status > -1);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categoryid"] = (int)product.CategoryId;
            ViewData["level"] = (int)product.Level;
            ViewData["status"] = (int)product.Status;
            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["load"] = 1;
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create([Bind("Name,Price,OriginalPrice,FilterPrice,InstallFee,Storage,CategoryId,Level,Status,VideoSrc,ImageUrl,Service,Summary,Description,Remark")] Product product)
        {

            product.CreateTime = DateTime.Now;
            product.UpdateTime = DateTime.Now;
            product.PaymentType = PaymentEnum.Buy;
            ViewData["categoryid"] = (int)product.CategoryId;
            ViewData["level"] = (int)product.Level;
            ViewData["status"] = (int)product.Status;
            if (!string.IsNullOrWhiteSpace(product.Name) && product.Price > 0 && product.OriginalPrice > 0 && product.InstallFee > 0 && product.FilterPrice > 0 && product.Storage >= 0 && product.CategoryId > 0 && product.Level > 0)
            {
                if (product.Name.Length > 50)
                {
                    ViewBag.ErrorInfo = "�������Ȳ����Ϲ淶����������ύ";
                    return View(product);
                  
                }
                var model = _unitOfWork.ProductRepository.GetProduct(new Core.DTO.ProductDto() { CategoryId = (CategoryEnum)product.CategoryId, level = product.Level, PaymentType = PaymentEnum.Buy });
                if (model != null)
                {
                  
                    ViewBag.ErrorInfo = "�Ѵ���ͬ���͵���Ʒ�������ظ����";
                    return View(product);
                }
                _unitOfWork.ProductRepository.Insert(product);
                _unitOfWork.SaveChange();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorInfo = "���������Ϲ淶����������ύ";
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.FirstOrDefault(e => e.Id == id && e.Status > -1);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categoryid"] = (int)product.CategoryId;
            ViewData["level"] = (int)product.Level;
            ViewData["status"] = (int)product.Status;
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Name,Price,OriginalPrice,FilterPrice,InstallFee,Storage,CategoryId,Level,Status,VideoSrc,ImageUrl,Service,Summary,Description,Remark")] Product product)
        {
            Product entity = null;
            if (id > 0)
            {
                entity = _unitOfWork.ProductRepository.FirstOrDefault(e => e.Id == id && e.Status > -1);
                if (entity == null)
                    return NotFound();
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(product.Name) && product.Price > 0 && product.OriginalPrice > 0 && product.InstallFee > 0 && product.FilterPrice > 0 && product.Storage >= 0)
                {
                    if (product.Name.Length> 50)
                    {
                        ViewBag.ErrorInfo = "�������Ȳ����Ϲ淶����������ύ";
                        return View(product);
                    }
                    entity.Name = product.Name;
                    entity.Price = product.Price;
                    entity.OriginalPrice = product.OriginalPrice;
                    entity.FilterPrice = product.FilterPrice;
                    entity.InstallFee = product.InstallFee;
                    entity.Storage = product.Storage;
                    entity.Summary = product.Summary;
                    entity.Service = product.Service;
                    //һ�����͵���Ʒ��ֻ�������޸�
                    //entity.CategoryId = product.CategoryId;
                    //entity.Level = product.Level;
                    entity.Status = product.Status;
                    entity.VideoSrc = product.VideoSrc;
                    entity.ImageUrl = product.ImageUrl;
                    entity.Description = product.Description;
                    entity.Remark = product.Remark;
                    entity.UpdateTime = DateTime.Now;
                    _unitOfWork.ProductRepository.Update(entity, true);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorInfo = "���������Ϲ淶����������ύ";
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(product);

        }

        // GET: Product/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _unitOfWork.ProductRepository.FirstOrDefault(m => m.Id == id);
            product.Status = -1;
            _unitOfWork.ProductRepository.Update(product, true);
            return RedirectToAction("Index");
        }


        public IActionResult GetExportData(int categoryId)
        {
            try
            {
                var result = new List<Product>();
                Expression<Func<Product, bool>> expression = e => e.PaymentType == PaymentEnum.Buy && e.Status > -1;
                if (categoryId > 0)
                {
                    expression = e => e.CategoryId == (CategoryEnum)categoryId && e.PaymentType == PaymentEnum.Buy && e.Status > -1;
                }
                result = _unitOfWork.ProductRepository.GetAllList(expression).OrderByDescending(e=>e.Id).ToList();
                string sWebRootFolder = _host.WebRootPath;
                string sFileName = $"��Ʒ����{ DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"; // $"{Guid.NewGuid()}.xlsx";
                using (MemoryStream stream = new MemoryStream())
                {
                    IWorkbook work = ProductExport.GetProductExport(result, PaymentEnum.Buy);
                    stream.Seek(0, SeekOrigin.Begin);
                    work.Write(stream);
                    return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
                }

            }
            catch (Exception ex)
            {
                // this.Logger.WriteError(ex.Message);
                ViewBag.ErrorInfo = "Excel�����쳣��";
                return null;
            }
        }
        private bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.Any(e => e.Id == id);
        }
    }
}
