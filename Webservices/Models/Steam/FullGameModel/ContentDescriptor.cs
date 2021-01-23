using Newtonsoft.Json;
using System.Collections.Generic;
using Webservices.JsonConverters;

namespace Webservices.Models.Steam.FullGameModel
{
    public class ContentDescriptor
    {
        [JsonProperty("ids")]
        public List<int> IDS { get; set; }
        [JsonProperty("notes")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<string>))]
        public string Notes { get; set; }
    }
}