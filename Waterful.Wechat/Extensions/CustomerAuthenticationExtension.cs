using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Waterful.Wechat.Extensions
{
    public static class CustomerAuthenticationExtension
    {
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <param name="Identities"></param>
        /// <returns></returns>
        public static int DefaultCustomerId(this IEnumerable<ClaimsIdentity> Identities)
        {
            if (!Identities.Any())
            {
                throw new ArgumentNullException(nameof(Identities));
            }

            var idt = Identities.FirstOrDefault().
                Claims.SingleOrDefault(m => m.Type == ClaimTypes.NameIdentifier);

            int id = 0;
            int.TryParse(idt.Value, out id);
            return id;
        }
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="Identities"></param>
        /// <returns></returns>
        public static string DefaultCustomerOpenId(this IEnumerable<ClaimsIdentity> Identities)
        {
            if (!Identities.Any())
            {
                throw new ArgumentNullException(nameof(Identities));
            }

            var idt = Identities.FirstOrDefault().
                Claims.SingleOrDefault(m => m.Type == ClaimTypes.PrimarySid);

            return idt.Value;
        }

        /// <summary>
        /// 判断当前用户是否设置了手机号码
        /// </summary>
        /// <param name="Identities"></param>
        /// <returns></returns>
        public static bool PhoneExist(this IEnumerable<ClaimsIdentity> Identities)
        {
            return Identities.SelectMany(m => m.Claims).Any(m => m.Type == ClaimTypes.MobilePhone);
        }
    }
}
