using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using TestPoint.Data;
using TestPoint.Data.Repositories;

namespace TestEndpoint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddScoped<IDockerImageRepository, DockerImageRepository>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMetricServer();
            app.UseHttpMetrics(options =>
            {
                options.RequestCount.Enabled = false;

                options.RequestDuration.Histogram = Metrics.CreateHistogram("testpoint_http_request_duration_seconds", "Some help text",
                    new HistogramConfiguration
                    {
                        Buckets = Histogram.LinearBuckets(start: 1, width: 1, count: 64),
                        LabelNames = new[] { "code", "method" }
                    });
            });
            UpdateDatabase(app);
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(
            IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ImageContext>();
            context.Database.Migrate();
        }
    }
}
