
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
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
