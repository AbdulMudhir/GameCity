using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Category
    {
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }


    }
}
