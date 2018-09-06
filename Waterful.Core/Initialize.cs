using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Core
{
    public class DbInitializer
    {
        /// <summary>
        /// 初始化填充数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int Initialize(PomeloMySqlDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return -999;// DB has been seeded
            }
            var Users = new User[]
            {
                new User{UserName="admin",Password="123456",Name="管理员"},
                new User{UserName="test01",Password="123456",Name="管理员01"},
                new User{UserName="test02",Password="123456",Name="管理员02"},
                new User{UserName="test03",Password="123456",Name="管理员03"},
                new User{UserName="test04",Password="123456",Name="管理员04"},
                new User{UserName="test05",Password="123456",Name="管理员05"},
                new User{UserName="test06",Password="123456",Name="管理员06"},
                new User{UserName="test07",Password="123456",Name="管理员07"},
                new User{UserName="test08",Password="123456",Name="管理员08"},
                new User{UserName="test09",Password="123456",Name="管理员09"},
                new User{UserName="test10",Password="123456",Name="管理员10"},
            };
            foreach (User s in Users)
            {
                context.Users.Add(s);
            }
            return context.SaveChanges();
        }

    }
}