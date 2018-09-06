using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Back.ViewModels;
using Waterful.Core.Models;

namespace Waterful.Back.App
{
    public class InitMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, LoginVM>();
                //Enity与Dto映射
                //cfg.CreateMap<Menu, MenuDto>();
                //cfg.CreateMap<MenuDto, Menu>();
            });
        }
        //AutoMapper进行实体转换
        //public List<MenuDto> GetAllList()
        //{
        //    var menus = _menuRepository.GetAllList().OrderBy(it => it.SerialNumber);
        //    //使用AutoMapper进行实体转换
        //    return Mapper.Map<List<MenuDto>>(menus);
        //}
    }
}
