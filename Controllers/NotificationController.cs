using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;

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
            using (Metrics.CreateHistogram("testpoint_POST_notification_duration_seconds", "").NewTimer())
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
}