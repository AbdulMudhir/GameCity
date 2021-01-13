using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
   public class DealDate
    {
        public Guid DealDateId { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }
        public DateTime? ExpiringDate { get; set; }
        public bool LimitedTimeDeal { get; set; } = false;
        public bool Expired { get; set; } = false;
        public DateTime? ExpiredDate { get; set; }

    }
}
