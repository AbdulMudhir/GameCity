using Newtonsoft.Json;

namespace SteamAPI.Models
{
    public class CategoryModel
    {   [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}