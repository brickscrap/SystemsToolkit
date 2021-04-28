using FuelPOS.DebugTools;
using Newtonsoft.Json;
using POSFileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;
using TSGSystemsToolkit.CmdLine.TestFuncs;

namespace TSGSystemsToolkit.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            GaugeFileTests gauge = new ();
            gauge.RunGaugeTest();

            watch.Stop();
            Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }

        public static void ParserTests()
        {
            var output = Parser.LoadTRXFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\61111042.PU");

            JsonSerializerSettings settings = new ()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(output, Formatting.Indented, settings);

            File.WriteAllText(@"C:\Users\omgit\source\repos\FuelPOSToolkit\POSFileParser\Tests\trx.json", json);
        }

        public static void DebuggerTests()
        {
            DebugFileCreator debugger = new ();
            List<string> processes = new ()
            {
                "HTEC GEMPAY 1,S R X",
                "OPT,X"
            };

            List<string> output = debugger.GenerateFileString(processes, 24);

            foreach (var line in output)
            {
                Console.WriteLine(line);
            }

            debugger.CreateFile(output, @"C:\Users\omgit\source\repos\FuelPOSToolkit\FuelPOSDebugTools\");
        }

        public static void FTPTests()
        {

        }

        public static void StatDevParserTests(string filePath)
        {
            List<StatdevModel> data = new ();
            string[] files = Directory.GetFiles(filePath, "*.xml");

            foreach (var file in files)
            {

                XDocument doc = XDocument.Load(file);
                StatdevParser parser = new (doc);
                data.Add(parser.Parse(doc));
            }
        }
    }
}
