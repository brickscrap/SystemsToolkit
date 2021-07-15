using FuelPOS.TankTableTools.Helpers;
using FuelPOS.TankTableTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.TankTableTools
{
    internal class MaxVolHandler
    {
        private List<string> _serialisedChart = new();

        private List<MaxVolModel> _maxVols = new();

        public List<MaxVolModel> MaxVols
        {
            get { return _maxVols; }
            set { _maxVols = value; }
        }


        public List<MaxVolModel> Parse(List<string> maxVolChart)
        {
            maxVolChart.RemoveHeading();

            List<string> serialisedChart = new();

            foreach (var line in maxVolChart)
            {
                var maxVol = ParseLine(line);
                if (maxVol is not null)
                {
                    _maxVols.Add(ParseLine(line));
                }
            }

            return MaxVols;
        }

        private MaxVolModel ParseLine(string line)
        {
            var newLine = line.ToSemiColonSeparated();

            if (newLine.Length > 0)
            {
                if (char.IsDigit(newLine[0]))
                {
                    var csv = newLine.Split(';');

                    if (csv.Length == 3)
                    {
                        if (int.Parse(csv[2]) > 0)
                        {
                            MaxVolModel model = new()
                            {
                                TankNumber = csv[0],
                                Grade = csv[1],
                                Litres = csv[2]
                            };

                            return model;
                        }
                    } 
                }
            }

            return null;
        }
    }
}
