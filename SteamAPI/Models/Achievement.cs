using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamAPI.Models
{
    public class Achievement
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        public List<Highlight> Highlights { get; set; }
    }
}