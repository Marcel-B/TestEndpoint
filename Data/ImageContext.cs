using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestPoint.Data.Models;
using TestPoint.Extensions;

namespace TestPoint.Data
{
    public class ImageContext : DbContext
    {
        public DbSet<DockerImage> DockerImages { get; set; }
        ILogger<ImageContext> _logger;

        public ImageContext(
            ILogger<ImageContext> logger,
            DbContextOptions<ImageContext> context) : base(context)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            modelBuilder.Seed();
        }
    }
}
