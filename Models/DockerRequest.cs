using System.Text.Json.Serialization;

namespace TestPoint.Models
{
    public class DockerRequest
    {
        [JsonPropertyName("push_data")]
        public PushData PushData { get; set; }

        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; set; }

        public Repository Repository { get; set; }
    }
}
