
using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class SubModels
    {
        [JsonProperty("packageid")]
        public int PackageId { get; set; }
        [JsonProperty("percent_savings_text")]
        public string PercentSavingText { get; set; }
        [JsonProperty("percent_savings")]
        public int PercentSavings { get; set; }
        [JsonProperty("option_text")]
        public string OptionText { get; set; }
        [JsonProperty("option_description")]
        public string OptionDescription { get; set; }
        [JsonProperty("can_get_free_license")]
        public string CanGetFreeLicense { get; set; }
        [JsonProperty("is_free_license")]
        public bool IsFreeLicense { get; set; }
        [JsonProperty("price_in_cents_with_discount")]
        public int PriceInCentWithDiscount { get; set; }

    }
}