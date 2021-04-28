using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;

namespace FuelPOSToolkitCmdLineUI
{
    public static class ScriptSamples
    {
        public static void ReadStatDevWriteSpreadsheet()
        {
            string[] files = System.IO.Directory.GetFiles("C:\\surveys", "*.xml");

            StatdevParser parser = new ();
            List<StatdevModel> data = new ();

            foreach (var file in files)
            {
                XDocument doc = XDocument.Load(file);
                data.Add(parser.Parse(doc));
            }

            SpreadsheetWriter writer = new ();

            writer.CreateFuelPOSSurvey(data);
        }
    }
}
