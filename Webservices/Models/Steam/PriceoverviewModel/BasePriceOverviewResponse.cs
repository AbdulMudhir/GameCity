using Newtonsoft.Json;
using Webservices.JsonConverters;
using Webservices.Models.Steam.FullGameModel;

namespace Webservices.Models.Steam.PriceoverviewModel
{
    public class BasePriceOverviewResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<BasePriceOverviewData>))]
        public BasePriceOverviewData Data { get; set; }

    }
      public class BasePriceOverviewData
    {
        [JsonProperty("price_overview")]
        public PriceOverview PriceOverview { get; set; }
    }
}