using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;
using POSFileParser;

namespace FuelPOSToolkitCmdLineUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.LoadFile();


            Console.ReadLine();
        }
    }
}
