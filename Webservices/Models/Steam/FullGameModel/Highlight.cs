
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Highlight
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}