using FuelPOS.TankTableTools;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    public class GaugeFileTests
    {
        public void RunGaugeTest()
        {
            GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\Blakel01.cap");

            // GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\Solihu02.cal");

            var tables = table.Parse();
            // table.DisplayTablesInConsole();

            POSFileCreator.CreateFile(tables, @"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example");
        }
    }
}
