using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mincms.Models.Entity
{
    //用户
    [Table("User2")]
    public class User2
    {
        //完成(除多对多)
        //多对多文章 多对多Blog(暂时没有)
        public User2()
        {
            AddTime = DateTime.Now;
            State = 0;
        }
        [Key]
        public int Id { get; set; }
        //public int RoleID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "用户名")]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "昵称")]
        public string NickName { get; set; }
        //[Required]
        //public string NickName1 { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(20,ErrorMessage = "不符合规范")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 性别 女0 男1
        /// </summary>
        [Display(Name = "性别")]
        public Nullable<bool> Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        public Nullable<DateTime> Birthday { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        //[Required(ErrorMessage = "必填")]
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机")]
        public string Mobile { get; set; }
        /// <summary>
        /// 所在地 610101
        /// </summary>
        [Display(Name = "所在地")]
        public string Address { get; set; }
        ///// <summary>
        ///// 总积分
        ///// </summary>
        //[Display(Name = "所在地")]
        //public int TotalCredit { get; set; }
        /// <summary>
        /// 用户状态<br />
        /// 新 -2关闭 -1锁定 0未审核 1正常 2高级 || 旧 0正常，1锁定，2未通过邮件验证，3未通过管理员
        /// </summary>
        [Display(Name = "用户状态")]
        public short State { get; set; }
        /// <summary>
        /// 验证状态  默认0  邮箱1 手机2 xx4
        /// </summary>
        public Nullable<short> isState { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        [Display(Name = "上次登录时间")]
        public Nullable<DateTime> LastTime { get; set; }
        /// <summary>
        /// 登录失败次数(限制10次)配合登录时间验证
        /// </summary>
        [Display(Name = "登录失败次数")]
        public Nullable<short> LoginNum { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>
        [Display(Name = "注册IP")]
        public string RegIP { get; set; }
        /// <summary>
        /// 上次登录IP/考虑维护一个单独的表
        /// </summary>
        [Display(Name = "上次登录IP")]
        public string LoginIP { get; set; }
        //[ForeignKey("User_ID")]
        //public virtual ICollection<Role> Roles { get; set; }
        //[ForeignKey("User_ID")]
        //public virtual ICollection<ShopCart> ShopCarts { get; set; }
        //[ForeignKey("User_ID")]
        //public virtual ICollection<Address> Addresss { get; set; }

        //public virtual Role Role { get; set; }
        //关联类型 0 Close 1 Article 2 Blog
        //public int Type { get; set; }
        //引用数(思路 用多线程 定时扫描数量 录入  但是 如果是文章 博客 同时用tag  不知道谁是谁了就  解决办法 ArticleQuote BlogQuote)
        //public Nullable<int> Quote { get; set; }
        //public short SortID { get; set; }
        //public System.DateTime AddTime { get; set; }
        //public bool Closed { get; set; }
        //public virtual ICollection<Column> Columns { get; set; }
        
    }

}