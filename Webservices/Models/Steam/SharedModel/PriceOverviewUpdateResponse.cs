using System;
using Domain.DatabaseModel;

namespace Webservices.Models.Steam.FullGameModel
{
    public class PriceOverviewUpdateResponse
    {
        // double check this
        public DateTime DateReceived { get; set; }

        public PriceOverviewUpdateResponse()
        {
            DateReceived = DateTime.Now;
        }

        public GameDeal GameDeal { get; set; }

        public int SteamAppId { get; set; }

        public PriceOverview Priceoverview { get; set; }

        public bool IsFree { get; set; } = false;
        public bool New { get; set; } = false;

        public bool Available { get; set; } = true;

    }
}