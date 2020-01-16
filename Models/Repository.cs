using System.Text.Json.Serialization;

namespace com.b_velop.TestPoint.Models
{
    public class Repository
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("is_trusted")]
        public bool IsTrusted { get; set; }

        [JsonPropertyName("full_description")]
        public string FullDescription { get; set; }

        [JsonPropertyName("repo_url")]
        public string RepoUrl { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("is_official")]
        public bool IsOfficial { get; set; }

        [JsonPropertyName("is_private")]
        public bool IsPrivate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }

        [JsonPropertyName("star_count")]
        public int StarsCount { get; set; }

        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }

        [JsonPropertyName("date_created")]
        public int DateCreated { get; set; }

        [JsonPropertyName("repo_name")]
        public string RepoName { get; set; }
    }
}
