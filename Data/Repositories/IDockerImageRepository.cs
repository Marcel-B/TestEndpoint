using System.Threading.Tasks;
using TestPoint.Data.Models;

namespace TestPoint.Data.Repositories
{
    public interface IDockerImageRepository : IRepositoryBase<DockerImage>
    {
        Task<DockerImage> SelectByImageAsync(string repoName, string tag);
    }
}
