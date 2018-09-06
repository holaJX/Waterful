using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace mincms.Models.Entity
{
    public enum StateEnum
    {
        删除 = -3,
        关闭 = -2,
        草稿 = -1,
        审核 = 0,
        发布 = 1
    }
    public enum BannerType
    {
        首页 = 1
    }
    public enum CatType
    {
        默认 = 0,
        文章 = 1,
        商品 = 2,
    }
}