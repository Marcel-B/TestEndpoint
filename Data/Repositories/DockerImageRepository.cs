using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestPoint.Data.Models;

namespace TestPoint.Data.Repositories
{
    public class DockerImageRepository : RepositoryBase<DockerImage>, IDockerImageRepository
    {
        public DockerImageRepository(
            ImageContext db,
            ILogger<DockerImage> logger) : base(db, logger) { }

        public async Task<DockerImage> SelectByImageAsync(
            string repoName,
            string tag)
        {
            var image = await Db.DockerImages.FirstOrDefaultAsync(_ => _.RepoName == repoName && _.Tag == tag);
            return image;
        }
    }
}
