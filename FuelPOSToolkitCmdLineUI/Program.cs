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
