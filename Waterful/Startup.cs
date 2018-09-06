using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waterful.Models;
using Microsoft.EntityFrameworkCore;
//using MySQL.Data.EntityFrameworkCore.Extensions;

namespace Waterful
{
    public class Startup
    {
        //log4net
        //public static ILoggerRepository repository { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //log4net
            //repository = LogManager.CreateRepository("NETCoreRepository");
            //XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //services.AddDbContext<MySqlDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));
            services.AddEntityFrameworkMySql();
            services.AddDbContext<PomeloMySqlDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ////log4net
            //var log = LogManager.GetLogger(repository.Name, typeof(Startup));
            //log.Info("Program Start");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}


//namespace ConsoleApplication
//{
//    using System;
//    using Microsoft.Extensions.Configuration;

//    public class Program
//    {
//        public static void Main(string[] args)
//        {

//            var builder = new ConfigurationBuilder()
//            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//            var configuration = builder.Build();

//            string connectionString = configuration.GetConnectionString("SampleConnection");

//            // Create an employee instance and save the entity to the database
//            var entry = new User() { Name = "John", LastName = "Winston" };

//            using (var context = ContextFactory.Create(connectionString))
//            {
//                context.Add(entry);
//                context.SaveChanges();
//            }

//            Console.WriteLine($"Employee was saved in the database with id: {entry.Id}");
//        }
//    }
//}