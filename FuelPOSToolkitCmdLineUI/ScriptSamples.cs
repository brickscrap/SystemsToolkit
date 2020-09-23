using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;

namespace FuelPOSToolkitCmdLineUI
{
    public class ScriptSamples
    {
        public void ReadStatDevWriteSpreadsheet()
        {
            string[] files = System.IO.Directory.GetFiles("C:\\surveys", "*.xml");

            StatdevParser parser = new StatdevParser();
            List<StatdevModel> data = new List<StatdevModel>();

            foreach (var file in files)
            {
                XDocument doc = XDocument.Load(file);
                data.Add(parser.Parse(doc));
            }

            SpreadsheetWriter writer = new SpreadsheetWriter();

            writer.CreateFuelPOSSurvey(data);
        }
    }
}
