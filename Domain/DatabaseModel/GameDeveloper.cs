using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
   public class GameDeveloper
    {
        public Guid GameDeveloperId { get; set; }
        [Required]
        public Guid DeveloperId { get; set; }
        [Required]
        public Guid GameId { get; set; }

    }
}
