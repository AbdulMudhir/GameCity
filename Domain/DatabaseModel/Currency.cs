using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Currency
    {


        public Guid CurrencyId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        public string Symbole { get; set; }
    }
}
