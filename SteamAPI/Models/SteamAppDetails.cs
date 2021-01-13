using Newtonsoft.Json;
using SteamAPI.Utilities;
using System.Collections.Generic;


namespace SteamAPI.Models
{
    public class SteamAppDetails
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("steam_appid")]
        public int SteamAppID { get; set; }

        [JsonProperty("required_age")]
        [JsonConverter(typeof(StringToIntJSONConverter))]
        public int RequiredAge { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }
        [JsonProperty("detailed_description")]
        public string Description { get; set; }
        [JsonProperty("reviews")]
        public string Reviews { get; set; }
        [JsonProperty("header_image")]
        public string HeaderImage { get; set; }
        [JsonProperty("about_the_game")]
        public string About { get; set; }
        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("pc_requirements")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<SystemRequirement>))]
        public SystemRequirement PcRequirement { get; set; }
        [JsonProperty("mac_requirements")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<SystemRequirement>))]
        public SystemRequirement MacRequirement { get; set; }
        [JsonProperty("linux_requirements")]
        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<SystemRequirement>))]
        public SystemRequirement LinuxRequirement { get; set; }
        [JsonProperty("developers")]
        public List<string> Developers { get; set; }
        [JsonProperty("publishers")]
        public List<string> Publishers { get; set; }
        [JsonProperty("price_overview")]
        public PriceOverview PriceOverview { get; set; }
        [JsonProperty("packages")]
        public List<int> Packages { get; set; }
        [JsonProperty("package_groups")]
        public List<Package> PackageGroups { get; set; }
        [JsonProperty("platforms")]
        public Platform Platforms { get; set; }
        [JsonProperty("categories")]
        public List<CategoryModel> Categories { get; set; }
        [JsonProperty("genres")]
        public List<GenreModel> Genres { get; set; }
        [JsonProperty("screenshots")]
        public List<Screenshot> Screenshots { get; set; }
        [JsonProperty("movies")]
        public List<Movie> Movies { get; set; }
        [JsonProperty("achievements")]
        public Achievement Achievements { get; set; }
        [JsonProperty("release_date")]
        public DateModel ReleaseDate { get; set; }

        [JsonProperty("support_info")]
        public SupportInfo SupportInfo { get; set; }
        [JsonProperty("background")]
        public string Background { get; set; }
        [JsonProperty("content_descriptors")]
        public ContentDescriptor ContentDescriptor { get; set; }
        [JsonProperty("dlc")]
        public List<int> DLC { get; set; }




    }
}
