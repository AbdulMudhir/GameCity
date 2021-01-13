using Newtonsoft.Json;
using SteamAPI.Utilities;
using System;


namespace SteamAPI.Models.JsonDeserializeModel
{
    public class PriceOverviewDetailsJsonModel
    {

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<PriceOverviewDetailsJsonModel>))]
        public PriceOverViewJsonModel Data { get; set; }

    }

    public class PriceOverViewJsonModel
    {
        [JsonProperty("price_overview")]
        public PriceOverview PriceOverview { get; set; }
    }

}
