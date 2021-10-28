using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public static class POSFileCreator
    {
        public static void CreateTmsAofFile(List<TankTableModel> tankTables, string outputDirectory)
        {
            List<string> lines = new();

            lines.Add("[START_FILE]");
            lines.Add("[TANK_TABLE_INFO]");

            lines.AddRange(AddTankTables(tankTables));

            lines.Add("[END_TANK_TABLE_INFO]");
            lines.Add("[END_FILE]");

            Directory.CreateDirectory(outputDirectory);

            File.WriteAllLines($@"{outputDirectory}\TMS_AOF.INP", lines);
        }

        public static void CreateFuelPosSetupCsv(VdrRootFileParser parsedGaugeFile, string outputDirectory)
        {
            if (parsedGaugeFile.MaxVols is null || parsedGaugeFile.TankTables is null)
            {
                return;
            }

            if (parsedGaugeFile.TankTables.Count > 0 && parsedGaugeFile.MaxVols.Count > 0)
            {
                List<string> lines = new();
                lines.Add("Tank;Grade;Theoretical Cap;Operational Cap;Height;Stock Out");

                foreach (var tank in parsedGaugeFile.CsvOutput)
                {
                    lines.Add($"{tank.TankNumber};{tank.Grade};{tank.Theoretical};{tank.Operational};{tank.Height};{tank.StockOut}");
                }

                Directory.CreateDirectory(outputDirectory);

                File.WriteAllLines($@"{outputDirectory}\{parsedGaugeFile.SiteName}.csv", lines);
            }
        }

        private static List<string> AddTankTables(List<TankTableModel> tankTables)
        {
            List<string> output = new List<string>();

            foreach (var table in tankTables)
            {
                int i = 1;
                foreach (var mm in table.Measurements)
                {
                    output.Add($"MM{table.TankNumber},{i}={mm.Item1}");
                    output.Add($"LIT{table.TankNumber},{i}={mm.Item2}");
                    i++;
                }
            }

            return output;
        }
    }
}
