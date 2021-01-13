using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class GameGenre
    {
        public Guid GameGenreId { get; set; }
        [Required]
        public Guid GameId { get; set; }
         [Required]
        public Guid GenreId { get; set; }
    }
}
