using FuelPOS.TankTableTools;
using System.IO;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    public class GaugeFileTests
    {
        public void RunGaugeTest()
        {
            VdrRootFileParser parser = new (@"C:\Users\omgit\source\repos\TSGSystemsToolkit\FuelPOS.TankTableTools\Example\Broken Cross\Broken02.cap");

            // GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\Solihu02.cal");

            parser.Parse();
            // table.DisplayTablesInConsole();

            var outputDir = @"C:\Users\omgit\source\repos\TSGSystemsToolkit\FuelPOS.TankTableTools\Example\Broken Cross";

            POSFileCreator.CreateTmsAofFile(parser.TankTables, outputDir);
            POSFileCreator.CreateFuelPosSetupCsv(parser, outputDir);
        }

        public void ConvertInDirectory(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.cap");

            foreach (var file in files)
            {
                VdrRootFileParser parser = new(file);
                parser.Parse();
                var newDirectory = $"{directoryPath}\\{parser.SiteName}";

                if (parser.TankTables is not null)
                {
                    POSFileCreator.CreateTmsAofFile(parser.TankTables, newDirectory);
                    POSFileCreator.CreateFuelPosSetupCsv(parser, newDirectory);

                    
                }

                Directory.CreateDirectory(newDirectory);

                File.Move(file, $"{newDirectory}\\{Path.GetFileName(file)}");
            }
        }
    }
}
