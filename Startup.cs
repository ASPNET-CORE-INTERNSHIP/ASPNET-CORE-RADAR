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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(10000);
                opt.Cookie.IsEssential = true;
            });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);
            //Add service for accessing current HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

                    if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserNotLoggedInException)
                    {
                        await context.Response.WriteAsync("You have to be logged in<br><br>\r\n");
                    }
                    else if (exceptionHandlerPathFeature?.Error is ASPNETAOP.Aspect.UserPermissionNotEnoughException)
                    {
                        await context.Response.WriteAsync("You don't have the necessary permission in<br><br>\r\n");
                    }

                    await context.Response.WriteAsync(
                                                    "<a href=\"/\">Login</a><br>\r\n");
                    await context.Response.WriteAsync("</body></html>\r\n");
                    await context.Response.WriteAsync(new string(' ', 512));
                });
            });


            if (!env.IsDevelopment()) { app.UseHsts(); }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
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

