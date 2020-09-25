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
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // Parser.LoadFile();
            
            
            
            watch.Stop();
            Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
