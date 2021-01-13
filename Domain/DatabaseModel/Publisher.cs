using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Publisher
    {
        public Guid PublisherId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
