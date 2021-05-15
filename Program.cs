using ASPNETAOP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ASPNETAOP
{
    public class Program
    {
        public static IDictionary<Guid, Data> data = new Dictionary<Guid, Data>();
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("https://*:5001;http://*:5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
