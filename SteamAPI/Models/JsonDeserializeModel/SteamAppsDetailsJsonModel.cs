using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SteamAPI.Models.JsonDeserializeModel
{
    public class SteamAppsDetailsJsonModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamAppDetails Data { get; set; }

    }
}
