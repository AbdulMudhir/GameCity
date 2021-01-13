
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class SteamProfileSettings
    {
        [JsonProperty("currentIndex")]
        public int CurrentIndex { get; set; }
        [JsonProperty("previousIndex")]
        public int PreviousIndex { get; set; }
    }
}
