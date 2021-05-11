using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TSGSystemsToolkit.CmdLine
{
    public static class ScriptSamples
    {
        public static void ReadStatDevWriteSpreadsheet()
        {
            string[] files = System.IO.Directory.GetFiles("C:\\surveys", "*.xml");

            StatDevParser parser = new();
            List<StatdevModel> data = new();

            foreach (var file in files)
            {
                XDocument doc = XDocument.Load(file);
                data.Add(parser.Parse(doc));
            }

            // SpreadsheetWriter writer = new();

            // writer.CreateFuelPOSSurvey(data);
        }
    }
}
