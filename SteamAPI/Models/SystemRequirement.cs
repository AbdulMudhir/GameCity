
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class SystemRequirement
    {
        [JsonProperty("minimum")]
        public string Minimum { get; set; }

        [JsonProperty("recommended")]
        public string Recommended { get; set; }
    }
}