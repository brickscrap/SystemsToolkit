using FuelPOS.DebugTools;
using System;
using System.Collections.Generic;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    internal static class PosDebuggerTests
    {
        public static void Run()
        {
            DebugFileCreator debugger = new();
            List<string> processes = new()
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
    }
}
