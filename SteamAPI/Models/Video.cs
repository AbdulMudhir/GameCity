
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class Video
    {
        [JsonProperty("480")]
        public string Quality { get; set; }
        [JsonProperty("max")]
        public string Max { get; set; }
    }
}