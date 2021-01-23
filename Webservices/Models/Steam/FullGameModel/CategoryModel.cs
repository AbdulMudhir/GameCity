using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class CategoryModel
    {   [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}