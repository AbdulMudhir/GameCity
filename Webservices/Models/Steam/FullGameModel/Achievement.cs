using System.Collections.Generic;
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Achievement
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        public List<Highlight> Highlights { get; set; }
    }
}