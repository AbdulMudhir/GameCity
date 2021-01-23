
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class GenreModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}