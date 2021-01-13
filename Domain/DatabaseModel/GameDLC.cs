using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class GameDLC

    {
        public Guid GameDLCId { get; set; }
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public Guid DLCId { get; set; }
    }
}
