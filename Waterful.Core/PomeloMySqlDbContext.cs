using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Core
{
    public class PomeloMySqlDbContext : DbContext
    {

        public PomeloMySqlDbContext(DbContextOptions<PomeloMySqlDbContext> options)
            : base(options)
        { }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Aftersale> Aftersales { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUse> CouponUses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Captcha> Captchas { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Userchat> Userchats { get; set; }
        public DbSet<Userinfo> Userinfos { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>().HasKey(c => new { c.Id });
            builder.Entity<Address>()
                .HasOne(p => p.Customer)
                .WithMany(b => b.Addresses)
                .HasForeignKey(p => p.CusomerId);
            builder.Entity<Order>()
                .HasMany(m => m.OrderItems)
                .WithOne(m => m.Order)
                .HasForeignKey(m => m.OrderId);
            builder.Entity<Order>()
                .Property(x => x.Timestamp)
                .IsConcurrencyToken().IsRowVersion().IsConcurrencyToken();
            builder.Entity<Order>()
                .HasMany(m => m.Aftersales)
                .WithOne(m => m.Order)
                .HasForeignKey(m => m.OrderId);
            builder.Entity<OrderItem>()
                .HasOne(m => m.Product)
                .WithMany()
                .HasForeignKey(m => m.ProductId);

            base.OnModelCreating(builder);
        }
    }

    //public static class ContextFactory
    //{
    //    public static MySqlDbContext Create(string connectionString)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
    //        optionsBuilder.UseMySQL(connectionString);

    //        //Ensure database creation
    //        var context = new MySqlDbContext(optionsBuilder.Options);
    //        context.Database.EnsureCreated();

    //        return context;
    //    }
    //}
}