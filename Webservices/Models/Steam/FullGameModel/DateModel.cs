
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class DateModel
    {
        [JsonProperty("coming_soon")]
        public bool ComingSoon { get; set; }
        [JsonProperty("date")]
        public string ReleaseDate { get; set; }
    }
}