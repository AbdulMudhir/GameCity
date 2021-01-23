
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class SupportInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}