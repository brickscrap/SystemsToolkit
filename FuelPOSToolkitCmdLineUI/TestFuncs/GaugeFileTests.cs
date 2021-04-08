using System;
using System.Collections.Generic;
using System.Text;
using TankTableToolkit;

namespace FuelPOSToolkitCmdLineUI.TestFuncs
{
    public class GaugeFileTests
    {
        public void RunGaugeTest()
        {
            GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\chiswe02.cap");

            // GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\Solihu02.cal");

            var tables = table.Parse();
            // table.DisplayTablesInConsole();

            POSFileCreator.CreateFile(tables, @"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example");
        }
    }
}
