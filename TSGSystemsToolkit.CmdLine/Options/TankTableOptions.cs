using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Options
{
    [Verb("tanktables", HelpText = "Generate useful files from Veeder Root ouput")]
    public class TankTableOptions
    {
        [Option('g', "gaugefile", Required = true, SetName = "filePath",
            HelpText = "Path to the file extracted from a Veeder Root.")]
        public string GaugeFilePath { get; set; }

        [Option('d', "dir", Required = true, SetName = "dirPath",
            HelpText = "Path to a directory containing Veeder Root scripts - scans for and attempts to read any .txt, .cal or .cap files." +
            "When creating files, will scan the scripts for the site name, and place files in a directory of that name.")]
        public string DirectoryPath { get; set; }

        [Option('o', "output", HelpText = "Output directory - if not specified, files will be generated in the same directory as the gauge file. Has no effect if used with -d or --dir")]
        public string OutputPath { get; set; }

        [Option('f', "full", Required = false, HelpText = "Enable this flag if the gauge output contains multiple scripts")]
        public bool FullFile { get; set; }

        [Option('p', "fuelposfile", HelpText = "Creates a FuelPOS tank table file (TMS_AOF.INP)")]
        public bool CreateFuelPosFile { get; set; }

        [Option('c', "csv", HelpText = "Creates a CSV file containing tank setup information")]
        public bool CreateTankSetupFile { get; set; }

        [Usage(ApplicationAlias = "systk")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                    new Example("Convert a Veeder Root tank table file into a FuelPOS TMS_AOF.INP",
                        new TankTableOptions {GaugeFilePath = "C:\\Path\\To Your\\File\\file.cap", OutputPath = "C:\\Output\\Directory", FullFile = true, CreateFuelPosFile = true})
                };
            }
        }
    }
}
