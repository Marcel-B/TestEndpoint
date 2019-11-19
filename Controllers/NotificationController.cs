using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(
            ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task Post()
        {
            var content = string.Empty;
            using (var bodyStream = new StreamReader(Request.Body))
            {
                content = await bodyStream.ReadToEndAsync();
            }
            _logger.LogInformation(1212, $"Notification Request:\r\n--------\r\n{content}\r\n--------");
        }
    }
}