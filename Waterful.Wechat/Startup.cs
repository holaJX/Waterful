using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog.Extensions.Logging;
using Waterful.Core;
using Microsoft.AspNetCore.Http;
using Waterful.Wechat.WeiXinHandler;
using Microsoft.EntityFrameworkCore;
using GZ.Platform.Core.Options;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using Waterful.Wechat.Options;

namespace Waterful.Wechat
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


        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            //微信公众号配置
            services.Configure<WeixinConfigSetting>(Configuration.GetSection("WeiXin"));
            //文件配置
            services.Configure<FileOption>(Configuration.GetSection(nameof(FileOption)));
            //产品配置
            services.Configure<ProductOption>(Configuration.GetSection(nameof(ProductOption)));

            services.AddScoped<IdGenerationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add framework services.
            services.AddMvc();

            services.AddScoped<UnitOfWork>();
            services.AddDbContext<PomeloMySqlDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();

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

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/Passport/Auth/"),

            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
