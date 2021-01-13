using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class Video
    {
        [Required]
        public Guid VideoId { get; set; }
        public string? Title { get; set; }
        [Required]
        public Guid GameId { get; set; }
        public bool Highlight { get; set; }
        public string Thumbnail { get; set; }

        public List<VideoContent>   VideoContent { get; set; }
       
    }
}
