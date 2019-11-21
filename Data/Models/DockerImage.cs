namespace TestPoint.Data.Models
{
    public class DockerImage : Entity, IName
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Pusher { get; set; }
        public string Namespace { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string RepoUrl { get; set; }
        public string TimeHash { get; set; }
    }
}
