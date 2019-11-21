using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using TestPoint.Data.Models;
using TestPoint.Data.Repositories;
using TestPoint.Extensions;
using TestPoint.Models;

namespace TestPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DockerController : ControllerBase
    {
        private readonly ILogger<DockerController> _logger;
        private readonly IRepositoryWrapper _rep;

        public DockerController(
            IRepositoryWrapper rep,
            ILogger<DockerController> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<ActionResult> PostAsync(
            DockerRequest dockerRequest)
        {
            var sb = new StringBuilder();
            var dt = dockerRequest.PushData.PushedAt.ToDateTime();
            sb.AppendLine($"Docker push from {dockerRequest.PushData.Pusher} | {dt}");
            sb.Append($"{dockerRequest.Repository.RepoName}:{dockerRequest.PushData.Tag}");
            _logger.LogInformation(1212, $"{sb.ToString()}");

            var dockerImage = new DockerImage
            {
                Id = Guid.NewGuid(),
                Name = dockerRequest.Repository.Name,
                Namespace = dockerRequest.Repository.Namespace,
                Owner = dockerRequest.Repository.Owner,
                Pusher = dockerRequest.PushData.Pusher,
                RepoName = dockerRequest.Repository.RepoName,
                RepoUrl = dockerRequest.Repository.RepoUrl,
                Tag = dockerRequest.PushData.Tag,
                Updated = DateTime.Now,
                TimeHash = DateTime.Now.GetHashCode().ToString()
            };

            var old = await _rep.DockerImage.SelectByImageAsync(dockerRequest.Repository.RepoName, dockerRequest.PushData.Tag);

            if (old != null)
            {
                dockerImage.Id = old.Id;
                dockerImage.Updated = DateTime.Now;
                dockerImage.Pusher = dockerRequest.PushData.Pusher;
                _ = _rep.DockerImage.Update(dockerImage);
            }
            else
            {
                _ = await _rep.DockerImage.InsertAsync(dockerImage);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<DockerImage> GetDockerImageAsync(
            Guid id)
        {
            var result = await _rep.DockerImage.SelectAsync(id);
            return result;
        }
    }
}