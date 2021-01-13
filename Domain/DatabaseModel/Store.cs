using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Store
    {
        public Guid StoreId { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Logo { get; set; }

    }
}
