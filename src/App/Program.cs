﻿using Core;
using Core.Data;
using Core.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userMgr = (UserManager<AppUser>)services.GetRequiredService(typeof(UserManager<AppUser>));
                if (!userMgr.Users.Any())
                {
                    userMgr.CreateAsync(new AppUser { UserName = "admin", Email = "admin@us.com" }, "Apex@123admin");
                    userMgr.CreateAsync(new AppUser { UserName = "demo", Email = "demo@us.com" }, "Apex@user1");
                }

                var context = services.GetRequiredService<AppDbContext>();

                // load application settings from appsettings.json
                var app = services.GetRequiredService<IAppService<AppItem>>();
                AppConfig.SetSettings(app.Value);

                if (!context.BlogPosts.Any())
                {
                    services.GetRequiredService<IStorageService>().Reset();
                    context.Seed();
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseUrls("http://127.0.0.1:5000").UseStartup<Startup>();
    }
}