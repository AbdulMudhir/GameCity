using Newtonsoft.Json;
using System.Collections.Generic;
using Webservices.JsonConverters;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Package
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("selection_text")]
        public string SelectionText { get; set; }
        [JsonProperty("save_text")]
        public string SaveText { get; set; }

        [JsonProperty("display_type")]
        [JsonConverter(typeof(StringToIntJSONConverter))]
        public int DisplayType { get; set; }
        [JsonProperty("is_recurring_subscription")]
        public string IsRecurringSubscription { get; set; }
        [JsonProperty("subs")]
        public List<SubModels> Subs { get; set; }
    }
}
