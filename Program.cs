﻿using System;
using com.b_velop.TestPoint.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using TestPoint.Data;

namespace com.b_velop.TestEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = "nlog.config";
            var logger = NLogBuilder.ConfigureNLog(file).GetCurrentClassLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.AddConsole();
                   logging.SetMinimumLevel(LogLevel.Trace);
               })
                .ConfigureServices(services =>
                {
                    var pw = Environment.GetEnvironmentVariable("CON_STR");
                    var server = "sqlserver";
#if DEBUG
                    pw = "foo123bar!";
                    server = "localhost";
#endif
                    var conString = $"Server={server},1433;Database=DockerImage;User Id=sa;Password={pw}";

                    services.AddDbContext<ImageContext>(options =>
                    {
                        options.UseSqlServer(conString);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:80");
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();
    }
}
