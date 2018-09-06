using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fonour.Application.UserApp.Dtos;
using AutoMapper;
using Waterful.Core.Repository;
using Waterful.Core.Models;

namespace Waterful.Back.App
{
    public interface IUserAppService
    {
        //User CheckUser(string userName, string password);
        //List<Waterful.Core.Repository.LoginVM> GetList(int pageIndex, int pageSize, out int rowCount);
        //List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount);

        //UserDto InsertOrUpdate(UserDto dto);

        ///// <summary>
        ///// 根据Id集合批量删除
        ///// </summary>
        ///// <param name="ids">Id集合</param>
        //void DeleteBatch(List<Guid> ids);

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id">Id</param>
        //void Delete(Guid id);

        ///// <summary>
        ///// 根据Id获取实体
        ///// </summary>
        ///// <param name="id">Id</param>
        ///// <returns></returns>
        //UserDto Get(Guid id);
    }
    /// <summary>
    /// 用户管理服务
    /// </summary>
    public class UserAppService : IUserAppService
    {
        //用户管理仓储接口
        //private readonly IUserRepository _repository;

        ///// <summary>
        ///// 构造函数 实现依赖注入
        ///// </summary>
        ///// <param name="userRepository">仓储对象</param>
        //public UserAppService(IUserRepository userRepository)
        //{
        //    _repository = userRepository;
        //}

        //public User CheckUser(string userName, string password)
        //{
        //    return _repository.CheckUser(userName, password);
        //}

        //public List<LoginVM> GetList(int pageIndex, int pageSize, out int rowCount)
        //{
        //    //可以用mapper
        //    //var result=_repository.LoadPageList(pageIndex, pageSize, out rowCount);
        //    rowCount = _repository.Count();
        //    var result = _repository.SearchList(pageIndex, pageSize);
        //    return result;
        //}
        //public List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount)
        //{
        //    return Mapper.Map<List<UserDto>>(_repository.LoadPageList(startPage, pageSize, out rowCount, it => it.DepartmentId == departmentId, it => it.CreateTime));
        //}

        ///// <summary>
        ///// 新增或修改
        ///// </summary>
        ///// <param name="dto">实体</param>
        ///// <returns></returns>
        //public UserDto InsertOrUpdate(UserDto dto)
        //{
        //    if (Get(dto.Id) != null)
        //        _repository.Delete(dto.Id);
        //    var user = _repository.InsertOrUpdate(Mapper.Map<User>(dto));
        //    return Mapper.Map<UserDto>(user);
        //}
        ///// <summary>
        ///// 根据Id集合批量删除
        ///// </summary>
        ///// <param name="ids">Id集合</param>
        //public void DeleteBatch(List<Guid> ids)
        //{
        //    _repository.Delete(it => ids.Contains(it.Id));
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id">Id</param>
        //public void Delete(Guid id)
        //{
        //    _repository.Delete(id);
        //}

        ///// <summary>
        ///// 根据Id获取实体
        ///// </summary>
        ///// <param name="id">Id</param>
        ///// <returns></returns>
        //public UserDto Get(Guid id)
        //{
        //    return Mapper.Map<UserDto>(_repository.GetWithRoles(id));
        //}
    }
}
