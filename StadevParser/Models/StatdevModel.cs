﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StadevParser.Models
{
    public class StatdevModel
    {
        public StationInfoModel StationInfo { get; set; }
        public List<PCInfoModel> POS { get; set; }
        public List<PumpModel> Dispensers { get; set; }
    }
}
