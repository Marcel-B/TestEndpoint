using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using TestPoint.Extensions;
using TestPoint.Models;

namespace TestPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DockerController : ControllerBase
    {
        private readonly ILogger<DockerController> _logger;

        public DockerController(
            ILogger<DockerController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        public ActionResult PostAsync(
            DockerRequest dockerRequest)
        {
            var sb = new StringBuilder();
            var dt = dockerRequest.PushData.PushedAt.ToDateTime();
            sb.AppendLine($"Docker push from {dockerRequest.PushData.Pusher} | {dt}");
            sb.Append($"{dockerRequest.Repository.RepoName}:{dockerRequest.PushData.Tag}");
            _logger.LogError(1212, $"{sb.ToString()}");
            return Ok();
        }
    }
}