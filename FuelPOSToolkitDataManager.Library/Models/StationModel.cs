using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitDataManager.Library.Models
{
    public class StationModel
    {
        public string Id { get; set; }
        public string KimoceId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public int NumberOfPOS { get; set; }
    }
}
