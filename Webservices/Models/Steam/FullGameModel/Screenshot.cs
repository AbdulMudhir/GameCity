
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Screenshot
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("path_thumbnail")]
        public string PathThumbnail { get; set; }
        [JsonProperty("path_full")]
        public string PathFull { get; set; }
    }
}