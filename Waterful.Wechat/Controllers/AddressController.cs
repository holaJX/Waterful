using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Waterful.Core;
using Microsoft.AspNetCore.Authorization;
using Waterful.Wechat.ViewModels;
using Waterful.Wechat.Extensions;
using Waterful.Core.Models;

namespace Waterful.Wechat.Controllers
{
    /// <summary>
    /// 收货地址
    /// </summary>
    [Authorize]
    public class AddressController : Controller
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        public AddressController(ILogger<OrderController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public IActionResult Index()
        {
            var data = new List<AddressVM>();
            try
            {

                var cid = User.Identities.DefaultCustomerId();
                var result = _unitOfWork.AddressRepository.GetAllList(e => e.Status == 1 && e.CusomerId == cid).Select(i => new AddressVM()
                {
                    Id = i.Id,
                    Mobile = i.Mobile,
                    Name = i.Name,
                    Display = i.Display,
                    Sort = i.Sort,
                    Street = i.Street
                });
                var sort = result.Max(e => e.Sort);//.OrderByDescending(e => e.Sort).FirstOrDefault();
                ViewData["sort"] = sort;// sortModel == null ? 0 : sortModel.Sort;
                data = result.ToList();

            }
            catch (Exception ex)
            {
                //dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Index), ex);
            }
            return View(data);
        }
        public IActionResult GetAddress()
        {
            AjaxResult<AddressVM> dto = new AjaxResult<AddressVM>();
            try
            {

                var cid = User.Identities.DefaultCustomerId();
                var entity = _unitOfWork.AddressRepository.FirstOrDefault(e => e.Status == 1 && e.CusomerId == cid, o => o.OrderByDescending(e => e.Sort));
                if (entity != null)
                {
                    dto.data = new AddressVM() { Name = entity.Name, Display = entity.Display, Mobile = entity.Mobile };
                    dto.err = 1;
                    dto.msg = "加载成功";
                }
                else
                {
                    dto.err = 0;
                    dto.msg = "请求失败";
                }


            }
            catch (Exception ex)
            {
                //dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Index), ex);
            }
            return Ok(dto);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddressVM model)
        {
            AjaxResult<int> dto = new AjaxResult<int>();
            try
            {
                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Mobile) || string.IsNullOrEmpty(model.Street))
                {
                    dto.data = 0;
                    dto.err = 0;
                    dto.msg = "参数不符合规范，请检查后再提交";
                    return Ok(dto);
                }
                var cid = User.Identities.DefaultCustomerId();
                var entity = new Address()
                {
                    CreateTime = DateTime.Now,
                    CusomerId = cid,
                    Name = model.Name,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    Status = 1,
                    UpdateTime = DateTime.Now,
                    Sort = ToUnixTime(DateTime.Now),
                    AreaId = model.AreaId,
                    Display = model.Display + model.Street

                };
                var result = _unitOfWork.AddressRepository.Insert(entity, true);
                dto.data = result.Id;
                dto.err = 1;
                dto.msg = "添加成功";


            }
            catch (Exception ex)
            {

                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Create), ex);
            }
            return Ok(dto);
        }

        public IActionResult Edit(int? id)
        {
            var vm = new AddressVM();
            if (id > 0)
            {
                var cid = User.Identities.DefaultCustomerId();
                var entity = _unitOfWork.AddressRepository.FirstOrDefault(e => e.Id == id && e.CusomerId == cid);
                vm.Id = entity.Id;
                vm.Name = entity.Name;
                vm.Mobile = entity.Mobile;
                vm.Street = entity.Street;
                vm.AreaId = entity.AreaId;
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(AddressVM model)
        {
            AjaxResult<int> dto = new AjaxResult<int>();
            try
            {


                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Street) || string.IsNullOrEmpty(model.Mobile))
                {
                    dto.data = model.Id;
                    dto.err = 0;
                    dto.msg = "参数不符合规范，请检查后再提交！";
                    return Ok(dto);
                }
                var cid = User.Identities.DefaultCustomerId();
                var entity = _unitOfWork.AddressRepository.FirstOrDefault(e => e.Id == model.Id && e.CusomerId == cid);
                if (entity == null)
                {
                    dto.data = model.Id;
                    dto.err = 0;
                    dto.msg = "数据不存在！";
                    return Ok(dto);
                }
                //var cid = User.Identities.DefaultCustomerId();
                entity.Name = model.Name;
                entity.Mobile = model.Mobile;
                entity.Street = model.Street;
                entity.UpdateTime = DateTime.Now;
                entity.AreaId = model.AreaId;
                entity.Display = model.Display + model.Street;
                _unitOfWork.AddressRepository.Update(entity, true);
                dto.data = entity.Id;
                dto.err = 1;
                dto.msg = "修改成功";
            }
            catch (Exception ex)
            {

                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Edit), ex);
            }
            return Ok(dto);
        }

        [HttpPost]

        public IActionResult Delete(int Id)
        {
            AjaxResult<int> dto = new AjaxResult<int>();
            try
            {
                var cid = User.Identities.DefaultCustomerId();
                var entity = _unitOfWork.AddressRepository.FirstOrDefault(e => e.Id == Id && e.CusomerId == cid);
                if (entity == null)
                {
                    dto.data = Id;
                    dto.err = 0;
                    dto.msg = "数据不存在！";
                    return Ok(dto);
                }
                //软删除
                entity.Status = -1;
                _unitOfWork.AddressRepository.Update(entity, true);
                dto.data = entity.Id;
                dto.err = 1;
                dto.msg = "删除成功";

            }
            catch (Exception ex)
            {

                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(Delete), ex);
            }
            return Ok(dto);
        }

        public IActionResult SetDefault(int Id)
        {
            AjaxResult<int> dto = new AjaxResult<int>();
            try
            {
                if (Id < 0) { dto.data = Id; dto.err = 0; dto.msg = "参数不符合规范，请检查后再提交"; return Ok(dto); }
                var cid = User.Identities.DefaultCustomerId();
                var entity = _unitOfWork.AddressRepository.FirstOrDefault(e => e.Id == Id && e.CusomerId == cid);
                if (entity == null)
                {
                    dto.data = Id;
                    dto.err = 0;
                    dto.msg = "数据不存在！";
                    return Ok(dto);
                }
                //软删除
                entity.Sort = ToUnixTime(DateTime.Now);
                entity.UpdateTime = DateTime.Now;
                _unitOfWork.AddressRepository.Update(entity, true);
                dto.data = entity.Id;
                dto.err = 1;
                dto.msg = "设置成功";

            }
            catch (Exception ex)
            {

                dto.msg = "系统繁忙，请稍后再试。";
                _logger.LogError(nameof(SetDefault), ex);
            }
            return Ok(dto);
        }
        public long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
        }
    }
}