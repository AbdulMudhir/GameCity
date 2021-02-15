using System;
using Domain.DatabaseModel;

namespace Webservices.Models.Steam.FullGameModel
{
    public class PriceOverviewUpdateResponse
    {
        public GameDeal GameDeal { get; set; }

        public int SteamAppId { get; set; }

        public PriceOverview Priceoverview { get; set; }

    }
}