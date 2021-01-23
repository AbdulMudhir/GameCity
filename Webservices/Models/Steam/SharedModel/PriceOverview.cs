
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
   public class PriceOverview
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("initial")]
        public decimal Initial { get; set; }
        [JsonProperty("final")]
        public decimal Final { get; set; }
        [JsonProperty("discount_percent")]
        public decimal DiscountPercentage { get; set; }
        [JsonProperty("initial_formatted")]
        public string InitialFormat { get; set; }
        [JsonProperty("final_formatted")]
        public string FinalFormat { get; set; }
    }
}
