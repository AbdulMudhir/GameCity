using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Screenshot
    {
        public Guid ScreenshotId { get; set; }
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public string PathFull { get; set; }
        [Required]
        public string PathThumbnail { get; set; }
    }
}
