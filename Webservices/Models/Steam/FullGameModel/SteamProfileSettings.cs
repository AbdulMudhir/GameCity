
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class SteamProfileSettings
    {
        [JsonProperty("currentIndex")]
        public int CurrentIndex { get; set; }
        [JsonProperty("previousIndex")]
        public int PreviousIndex { get; set; }
    }
}
