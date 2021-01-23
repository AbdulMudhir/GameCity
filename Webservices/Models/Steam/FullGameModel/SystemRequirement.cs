
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class SystemRequirement
    {
        [JsonProperty("minimum")]
        public string Minimum { get; set; }

        [JsonProperty("recommended")]
        public string Recommended { get; set; }
    }
}