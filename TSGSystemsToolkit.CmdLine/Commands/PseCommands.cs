﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;
using Pse.TerminalsToEmis;

namespace TSGSystemsToolkit.CmdLine.Commands
{
    public static class PseCommands
    {
        internal static void RunTerminalsCommands(TerminalsOptions opts)
        {
            if (opts.CreateEmisFile)
            {
                string outputPath;

                if (string.IsNullOrWhiteSpace(opts.OutputPath))
                {
                    outputPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    outputPath = $"{outputPath}\\FuelPos";
                }
                else
                {
                    outputPath = opts.OutputPath;
                }

                TerminalsToEmis.Run($"{opts.FilePath}\\Terminals_044.csv", outputPath);
            }
        }
    }
}
