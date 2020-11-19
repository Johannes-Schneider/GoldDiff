using Newtonsoft.Json;

namespace GoldDiff.GitHub.RemoteApi
{
    public sealed class GitHubReleaseInfo
    {
        [JsonProperty("tag_name")]
        public string Version { get; set; }
        
        [JsonProperty("html_url")]
        public string Url { get; set; }
        
        [JsonProperty("assets")]
        public GitHubReleaseAsset[] Assets { get; set; }
    }
}