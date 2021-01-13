using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class VideoContent
    {
        public Guid VideoContentId { get; set; }

        public Guid VideoId { get; set; }
        [Required]
        public string Quality { get; set; }
        [Required]
        public string Max { get; set; }
        [Required]
        public string MediaType { get; set; }
    }
}
