using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Game
    {
        public Guid GameID { get; set; }
        [MaxLength(20000)]
        public string Title { get; set; }
        public string Type { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }

        public List<GameDeal> GameDeals { get; set; }
        public ReleaseDate? ReleaseDate { get; set; }
        public Guid? ReleaseDateID { get; set; }
        public string HeaderImage { get; set; }
        public string Background { get; set; }
        public Guid? SteamAppId { get; set; }
        [Required]
        public SteamApp? SteamApp { get; set; }





    }
}
