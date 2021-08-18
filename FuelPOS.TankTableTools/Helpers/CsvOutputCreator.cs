using FuelPOS.TankTableTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools.Helpers
{
    public static class CsvOutputCreator
    {
        public static List<CsvOutputModel> Create(List<TankTableModel> tankTables, List<MaxVolModel> maxVols)
        {
            // TODO: If there's a tank with a grade but no measurements, an exception occurs
            // Joins list of tank tables and max volumes on the tank number
            List<CsvOutputModel> output = tankTables.Join(maxVols,
                tank => tank.TankNumber,
                maxVol => maxVol.TankNumber,
                (tank, maxVol) => new CsvOutputModel
                {
                    TankNumber = tank.TankNumber,
                    Height = tank.Measurements.Last().Item1.ToString(),
                    Grade = maxVol.Grade,
                    Theoretical = tank.Measurements.Last().Item2.ToString(),
                    Operational = maxVol.Litres,
                    StockOut = tank.StockOut
                }).ToList();

            return output;
        }
    }
}
