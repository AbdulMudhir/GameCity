using Newtonsoft.Json;
using SteamAPI.Utilities;
using System.Collections.Generic;

namespace SteamAPI.Models
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