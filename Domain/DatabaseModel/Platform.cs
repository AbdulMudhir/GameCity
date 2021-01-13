using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Platform
    {
        public Guid PlatformId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
