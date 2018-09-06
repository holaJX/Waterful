using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Waterful.Core.Enums
{

    public static class OrderEnum
    {

        public static IDictionary<int, string> OrderDic()
        {
            IDictionary<int, string> d = new Dictionary<int, string>();
            d.Add(-2, "删除");
            d.Add(-1, "订单取消");
            d.Add(0, "未支付");
            d.Add(1, "已支付");
            d.Add(2, "完成");
            return d;
        }

    }
}
