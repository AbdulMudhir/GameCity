using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DatabaseModel
{
    public class DLC
    {
        public Guid DLCId { get; set; }
        public int SteamAppID { get; set; }
        public string? Title { get; set; }
     
    }
}
