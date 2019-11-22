using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Prometheus;
using TestPoint.Data;

namespace TestEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = "nlog.config";
            var logger = NLogBuilder.ConfigureNLog(file).GetCurrentClassLogger();
            var stage = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
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
            try
            {
                if (stage != "Development")
                {
                    var metricServer = new MetricPusher(
                        endpoint: "https://push.qaybe.de/metrics",
                        job: "testpoint");
                    metricServer.Start();
                }
            }catch(Exception e)
            {
                logger.LogException(NLog.LogLevel.Error, "Error occurred while starting MetricPusher", e);
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
#if DEBUG
                    pw = "foo123bar!";
#endif
                    var conString = $"Server=sqlserver,1433;Database=DockerImage;User Id=sa;Password={pw}";

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
