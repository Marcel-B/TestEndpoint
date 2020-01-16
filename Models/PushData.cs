using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace com.b_velop.TestPoint.Models
{
    public class PushData
    {
        [JsonPropertyName("pushed_at")]
        public int PushedAt { get; set; }

        [JsonPropertyName("images")]
        public List<string> Images { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("pusher")]
        public string Pusher { get; set; }
    }
}
