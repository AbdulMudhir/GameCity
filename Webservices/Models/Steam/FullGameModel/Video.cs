
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Video
    {
        [JsonProperty("480")]
        public string Quality { get; set; }
        [JsonProperty("max")]
        public string Max { get; set; }
    }
}