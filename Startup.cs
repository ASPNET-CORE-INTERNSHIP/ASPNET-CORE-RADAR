using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

//runs on localhost:44363
namespace ASPNETAOP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            //Required for Sessions 
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(10000);
                opt.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            /*var connStr = Configuration.GetConnectionString("RadarDB");

            services.AddNHibernate(connStr);
            services.AddControllersWithViews();*/
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/html";

                    await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                    await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    //Handling possible errors from Aspects
                    if (exceptionHandlerPathFeature?.Error is Aspect.UserNotLoggedInException)
                    {
                        await context.Response.WriteAsync("You have to be logged in<br><br>\r\n");
                    }
                    
                    if (exceptionHandlerPathFeature?.Error is Aspect.UserPermissionNotEnoughException)
                    {
                        await context.Response.WriteAsync("You don't have the necessary permission in<br><br>\r\n");
                    }
                    
                    if (exceptionHandlerPathFeature?.Error is Aspect.UserSessionExpiredException)
                    {
                        await context.Response.WriteAsync("Session has expired in<br><br>\r\n");
                    }

                    await context.Response.WriteAsync(
                                                    "<a href=\"/\">Login</a><br>\r\n");
                    await context.Response.WriteAsync("</body></html>\r\n");
                    await context.Response.WriteAsync(new string(' ', 512));
                });
            });

            if (!env.IsDevelopment()) { app.UseHsts(); }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            AppHttpContext.Services = app.ApplicationServices;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserLogin}/{action=Login}/{id?}");
            });
        }
    }
}

