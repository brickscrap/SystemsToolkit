using System.Collections.Generic;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;

namespace FuelPOSToolkitCmdLineUI
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument doc1 = XDocument.Load("99612_statdev.xml");
            XDocument doc2 = XDocument.Load("L2330_statdev.xml");

            List<StatdevModel> data = new List<StatdevModel>();

            StatdevParser parser = new StatdevParser();
            var statDev1 = parser.Parse(doc1);
            var statDev2 = parser.Parse(doc2);

            data.Add(statDev1);
            data.Add(statDev2);

            SpreadsheetWriter writer = new SpreadsheetWriter();

            writer.CreateFuelPOSSurvey(data);
        }
    }
}
