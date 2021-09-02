using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools.Helpers
{
    public static class TableCreator
    {
        public static  TankTableModel CreateValuePairs(this TankTableModel tankTable, List<string> line)
        {
            for (int i = 0; i < line.Count; i = i + 2)
            {
                double mm = double.Parse(line[i].Trim());
                double litres = double.Parse(line[i + 1].Trim());
                tankTable.Measurements.Add((mm, litres));
            }

            return tankTable;
        }
    }
}
