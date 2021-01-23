
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class BaseSteamResponse
    {
        
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public SteamAppDetails Data { get; set; }

        
    }
}