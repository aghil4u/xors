using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Verification
    {

        [Key] public int id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string User { get; set; }
        public string AssetNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string AssetDescription { get; set; }
        public long TimeStamp { get; set; }

    }
}
