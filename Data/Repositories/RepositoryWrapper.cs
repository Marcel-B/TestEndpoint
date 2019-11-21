using Microsoft.Extensions.Logging;

namespace TestPoint.Data.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ILogger<RepositoryWrapper> _logger;
        protected ImageContext Db { get; set; }

        public IDockerImageRepository DockerImage { get; set; }

        public RepositoryWrapper(
            ImageContext db,
            IDockerImageRepository dockerImage,
            ILogger<RepositoryWrapper> logger)
        {
            _logger = logger;
            DockerImage = dockerImage;
            Db = db;
        }
    }
}
