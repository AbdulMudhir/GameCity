using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Domain.DatabaseModel
{
    public class PriceOverview
    {
        public Guid PriceOverviewId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string PriceFormat { get; set; }
        [Required]
        public decimal FinalPrice { get; set; }
        [Required]
        public string FinalPriceFormat { get; set; }

        [Required]
        public Currency Currency { get; set; }
        public Guid CurrencyId { get; set; }

        [Required]
        public decimal DiscountPercentage { get; set; }


    }
}
