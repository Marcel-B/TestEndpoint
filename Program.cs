using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using TestPoint.Data;

namespace TestEndpoint
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
                    var conString = System.Environment.GetEnvironmentVariable("CON_STR");
#if DEBUG
                    conString = "Server=localhost,1433;Database=DockerImage;User Id=sa;Password=foo123bar!";
#endif
                    services.AddDbContext<ImageContext>(options =>
                    {
                        options.UseSqlServer(conString);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();
    }
}
