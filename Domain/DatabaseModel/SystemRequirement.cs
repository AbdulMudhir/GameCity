using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class SystemRequirement
    {
        public Guid SystemRequirementId { get; set; }
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public string Minimum { get; set; }
        public string Recommended { get; set; }
    }
}
