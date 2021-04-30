using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public static class POSFileCreator
    {
        public static void CreateFile(List<TankTableModel> tankTables, string outputDirectory)
        {
            List<string> lines = new List<string>();

            lines.Add("[START_FILE]");
            lines.Add("[TANK_TABLE_INFO]");

            lines.AddRange(AddTankTables(tankTables));

            lines.Add("[END_TANK_TABLE_INFO]");
            lines.Add("[END_FILE]");

            string[] output = lines.ToArray();

            File.WriteAllLines($@"{outputDirectory}\TMS_AOF.INP", lines);
        }
        private static List<string> AddTankTables(List<TankTableModel> tankTables)
        {
            List<string> output = new List<string>();

            foreach (var table in tankTables)
            {
                int i = 1;
                foreach (var mm in table.Measurements)
                {
                    output.Add($"MM{table.TankNumber},{i}={mm.Item1}.00");
                    output.Add($"LIT{table.TankNumber},{i}={mm.Item2}.00");
                    i++;
                }
            }

            return output;
        }
    }
}
