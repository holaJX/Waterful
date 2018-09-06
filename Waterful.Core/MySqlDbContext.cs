using Microsoft.EntityFrameworkCore;
//using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Core
{
    //public class MySqlDbContext : DbContext
    //{
    //    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
    //        : base(options)
    //    { }
    //    public DbSet<Address> Addresss { get; set; }
    //    public DbSet<Aftersale> Aftersales { get; set; }
    //    public DbSet<Coupon> Coupons { get; set; }
    //    public DbSet<CouponUse> CouponUses { get; set; }
    //    public DbSet<Order> Orders { get; set; }
    //    public DbSet<OrderItem> OrderItems { get; set; }
    //    public DbSet<Product> Products { get; set; }
    //    public DbSet<User> Users { get; set; }
    //    public DbSet<Worker> Workers { get; set; }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {

    //        base.OnModelCreating(builder);
    //    }
    //}

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