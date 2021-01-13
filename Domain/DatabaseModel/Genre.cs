using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Genre
    {
        public Guid GenreId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
