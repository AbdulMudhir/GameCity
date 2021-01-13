using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class GamePublisher
    {

        public Guid GamePublisherId { get; set; }

        public Guid PublisherId { get; set; }
        [Required]
        public Guid GameId { get; set; }
    }
}
