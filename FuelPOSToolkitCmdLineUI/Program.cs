using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;
using POSFileParser;
using TankTableToolkit;

namespace FuelPOSToolkitCmdLineUI
{
    class Program
    {
        static void Main(string[] args)
        {
            GaugeFileParser table = new GaugeFileParser(@"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example\Aqueduct 200922d.cal");

            var tables = table.Parse();
            table.DisplayTablesInConsole();

            POSFileCreator.CreateFile(tables, @"C:\Users\omgit\source\repos\FuelPOSToolkit\TankTableToolkit\Example");
            // Parser.LoadFile();


            Console.ReadLine();
        }
    }
}
