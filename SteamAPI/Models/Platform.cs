
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class Platform
    {
        [JsonProperty("windows")]
        public bool Window { get; set; }

        [JsonProperty("Mac")]
        public bool Mac { get; set; }

        [JsonProperty("Linux")]
        public bool Linux { get; set; }
    }
}
