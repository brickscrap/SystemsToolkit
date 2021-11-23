using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    internal static class StatdevParserTests
    {
        public static void Run(string filePath)
        {
            List<StatdevModel> data = new();
            string[] files = Directory.GetFiles(filePath, "*.xml");

            foreach (var file in files)
            {
                XDocument doc = XDocument.Load(file);
                StatDevParser parser = new(doc);
                data.Add(parser.Parse(doc));
            }
        }
    }
}
