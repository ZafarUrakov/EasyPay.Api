//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EasyPay.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
