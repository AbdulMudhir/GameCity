
using Newtonsoft.Json;

namespace Webservices.Models.Steam.FullGameModel
{
    public class Movie
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonProperty("webm")]
        public Video Webm { get; set; }
        [JsonProperty("mp4")]
        public Video MP4 { get; set; }
        [JsonProperty("highlight")]
        public bool Highlight { get; set; }
    }
}