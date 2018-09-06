using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Waterful.Core;
using Waterful.Core.Enums;

namespace Waterful.Wechat.Extensions
{
    public class CouponHelper
    {

        private static ConcurrentQueue<int> djQueue = new ConcurrentQueue<int>();
        private static ConcurrentQueue<int> mazQueue = new ConcurrentQueue<int>();
        private static SpinLock djslock = new SpinLock();
        private static DateTime djsdt = DateTime.Now;
        private static SpinLock mazslock = new SpinLock();
        private static DateTime mazsdt = DateTime.Now;

        /// <summary>
        /// 代金券1 免安装4
        /// </summary>
        /// <param name="couponType"></param>
        /// <returns></returns>
        public static int Get(UnitOfWork _unitOfWork, CouponTypeEnum couponType)
        {
            int id = 0;
            if (couponType == CouponTypeEnum.Used)
            {
                if (!djQueue.TryDequeue(out id))
                {
                    DateTime dt = DateTime.Now;
                    if ((dt - djsdt).Seconds > 5)
                    {
                        bool lockTaken = false;
                        try
                        {
                            djslock.Enter(ref lockTaken);
                            if (djQueue.IsEmpty)
                            {
                                int status = 1;
                                var coupon = _unitOfWork.CouponRepository.CouponIds(1, 3, e => e.CouponType == CouponEnum.OnLine && e.Type == couponType && e.Status == status && e.CustomerId == id && e.ExpiryDate >= dt);
                                foreach (var item in coupon)
                                {
                                    djQueue.Enqueue(item);
                                }
                                djsdt = dt;
                            }
                        }
                        finally
                        {
                            if (lockTaken)
                                djslock.Exit(false);
                        }
                        djQueue.TryDequeue(out id);
                    }
                }
            }
            else if (couponType == CouponTypeEnum.FreeInstall)
            {
                if (!mazQueue.TryDequeue(out id))
                {
                    DateTime dt = DateTime.Now;
                    if ((dt - mazsdt).Minutes > 5)
                    {
                        bool lockTaken = false;
                        try
                        {
                            mazslock.Enter(ref lockTaken);
                            if (mazQueue.IsEmpty)
                            {
                                int status = 1;
                                var coupon = _unitOfWork.CouponRepository.CouponIds(1, 50, e => e.CouponType == CouponEnum.OnLine && e.Type == couponType && e.Status == status && e.CustomerId == id && e.ExpiryDate >= dt);
                                foreach (var item in coupon)
                                {
                                    mazQueue.Enqueue(item);
                                }
                                mazsdt = dt;
                            }
                        }
                        finally
                        {
                            if (lockTaken)
                                mazslock.Exit(false);
                        }
                        mazQueue.TryDequeue(out id);
                    }

                }

            }
            return id;
        }
    }
}
