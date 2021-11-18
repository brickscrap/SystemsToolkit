﻿using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SurveyOptions
    {
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool CreateSpreadsheet { get; set; }
    }
}
