using Newtonsoft.Json;

namespace GoldDiff.GitHub.RemoteApi
{
    public sealed class GitHubReleaseAsset
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        
        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}