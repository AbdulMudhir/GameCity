using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DatabaseModel
{
    public class GameCategory
    {
        public Guid GameCategoryId { get; set; }
        public Guid GameId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
