using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
   public class Developer
    {
        public Guid DeveloperId { get; set; }
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

    }
}
