using System;
using Domain.DatabaseModel;

namespace Webservices.Models.Steam.FullGameModel
{
    public class PriceOverviewUpdateResponse
    {
        public GameDeal GameDeal { get; set; }

        public int SteamAppId { get; set; }

        public PriceOverview Priceoverview { get; set; }

        public bool IsFree { get; set; } = false;
        public bool IsNewCurrency { get; set; } = false;

        public bool Available { get; set; } = true;

    }
}