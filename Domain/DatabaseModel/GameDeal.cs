using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class GameDeal
    {
        public Guid GameDealId { get; set; }
        [Required]
        public string Url { get; set; }

        public Game Game {get;set;}

        [Required]
        public Store Store { get; set; }
        public Guid StoreId { get; set; }
        [Required]
        public Guid GameId { get; set; }
        public Guid? PriceOverviewId { get; set; }
        public PriceOverview? PriceOverview { get; set; }
        public Guid DealDateId { get; set; }
        [Required]
        public DealDate DealDate { get; set; }
        public bool IsFree { get; set; } = false;

    }
}
