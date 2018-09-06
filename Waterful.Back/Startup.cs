using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waterful.Core;
//using MySQL.Data.EntityFrameworkCore.Extensions;
using System.IO;
using Waterful.Core.Repository;
using Waterful.Back.App;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NLog.Web;
using GZ.Platform.Core.Options;

namespace Waterful.Back
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("nlog.config");
            //初始化映射关系
            InitMapper.Initialize();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //依赖注入
            //services.AddScoped<IAddressRepository, AddressRepository>();
            //services.AddScoped<IAftersaleRepository, AftersaleRepository>();
            ////services.AddScoped<ICommentRepository, CommentRepository>();
            //services.AddScoped<ICouponRepository, CouponRepository>();
            //services.AddScoped<ICouponUseRepository, CouponUseRepository>();
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();
            //services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IWorkerRepository, WorkerRepository>();

            //services.AddScoped<IUserAppService, UserAppService>();

            //services.AddMvc(options => options.MaxModelValidationErrors = 500);

            services.AddScoped<UnitOfWork>();
            services.AddScoped<IdGenerationService>();

            //文件配置
            services.AddOptions().Configure<FileOption>(Configuration.GetSection(nameof(FileOption)));

            // Add framework services.
            services.AddMvc();
            //增加Session服务注册
            services.AddSession();
            //数据库上下文
            //services.AddDbContext<PomeloMySqlDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection"), b => b.MigrationsAssembly("Waterful.Back")));
            services.AddDbContext<PomeloMySqlDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection"), b => b.MigrationsAssembly("Waterful.Back")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                //开发环境异常处理
                app.UseDeveloperExceptionPage();
                //
                app.UseBrowserLink();
            }
            else
            {
                //生产环境异常处理
                app.UseExceptionHandler("/Home/Error");
            }
            //使用静态文件
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Directory.GetCurrentDirectory())
            });
            //请求管道中启用Session
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
