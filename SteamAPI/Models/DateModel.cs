
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class DateModel
    {
        [JsonProperty("coming_soon")]
        public bool ComingSoon { get; set; }
        [JsonProperty("date")]
        public string ReleaseDate { get; set; }
    }
}