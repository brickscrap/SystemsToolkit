using System;
using System.Collections.Generic;
using System.Text;

namespace StadevParser.Models
{
    public class StationInfoModel
    {
        public string StationNumber { get; set; }
        public string StationName { get; set; }
        public string Company { get; set; }
        public int NumberOfTills { get; set; }
    }
}
