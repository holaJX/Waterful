using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waterful.Models
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
        { }
        //public DbSet<Article> Articles { get; set; }
        //public DbSet<Banner> Banners { get; set; }
        //public DbSet<Category> Categorys { get; set; }
        //public DbSet<Goods> Goods { get; set; }

        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<MySqlDbContext>(modelBuilder);
        //    Database.SetInitializer(sqliteConnectionInitializer);
        //    //映射到数据库中的表 !!!!!!
        //    //modelBuilder.Entity<User>().ToTable("User");
        //}
    }
    public static class ContextFactory
    {
        public static MySqlDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            //Ensure database creation
            var context = new MySqlDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}