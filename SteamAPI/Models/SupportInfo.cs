
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class SupportInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}