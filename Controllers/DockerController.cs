﻿using com.b_velop.TestPoint.Data.Repositories;
using com.b_velop.TestPoint.Extensions;
using com.b_velop.TestPoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPoint.Data.Models;

namespace com.b_velop.TestPoint.Controllers
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

        [HttpPost]
        public async Task<ActionResult> PostAsync(
            DockerRequest dockerRequest)
        {
            using (Metrics.CreateHistogram("testpoint_POST_docker_duration_seconds", "").NewTimer())
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
                    TimeHash = Guid.NewGuid().ToString()
                };

                var old = await _rep.DockerImage.SelectByImageAsync(dockerRequest.Repository.RepoName, dockerRequest.PushData.Tag);

                if (old != null)
                {
                    dockerImage.Id = old.Id;
                    old.Updated = DateTime.Now;
                    old.Pusher = dockerRequest.PushData.Pusher;
                    old.Tag = dockerRequest.PushData.Tag;
                    old.TimeHash = Guid.NewGuid().ToString();
                    _ = _rep.DockerImage.Update(old);
                }
                else
                {
                    _ = await _rep.DockerImage.InsertAsync(dockerImage);
                }
                return Ok();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<DockerImage>> GetDockerImageAsync()
        {
            using (Metrics.CreateHistogram("testpoint_GET_docker_duration_seconds", "").NewTimer())
            {
                var result = await _rep.DockerImage.SelectAllAsync();
                return result.OrderByDescending(_ => _.Updated);
            }
        }

        [HttpGet("{id}")]
        public async Task<DockerImage> GetDockerImageAsync(
            Guid id)
        {
            using (Metrics.CreateHistogram("testpoint_GET_docker_id_duration_seconds", "").NewTimer())
            {
                var result = await _rep.DockerImage.SelectAsync(id);
                return result;
            }
        }
    }
}
