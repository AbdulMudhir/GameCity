using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.DatabaseModel
{
    public class SteamApp
    {
        
        public Guid SteamAppId { get; set; }
        public string SteamReview { get; set; }
        public int SteamReviewCount { get; set; }

        public int SteamId { get; set; }

        public int? SteamIdLinkedTo { get; set; }

        public bool ValidSteamId { get; set; } =true;
    }
}
