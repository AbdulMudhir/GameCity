using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class ReleaseDate
    {
        public Guid ReleaseDateId { get; set; }
        [Required]
        public bool ComingSoon { get; set; }
        [Required]
        public string ReleasedDate { get; set; }
    }
}
