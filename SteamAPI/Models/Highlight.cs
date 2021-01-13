
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class Highlight
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}