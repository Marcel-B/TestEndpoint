namespace TestPoint.Data.Repositories
{
    public interface IRepositoryWrapper
    {
        IDockerImageRepository DockerImage { get; set; }
    }
}
