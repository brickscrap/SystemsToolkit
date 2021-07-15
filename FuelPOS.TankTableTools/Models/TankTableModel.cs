using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankTableToolkit.Models
{
    public class TankTableModel
    {
        private List<(double, double)> _measurements;

        public string TankNumber { get; set; }

        public string StockOut
        {
            get
            {
                var stockOut = Measurements.Where(x => x.Item1 == 250).FirstOrDefault().Item2.ToString();
                return stockOut;
            }
        }

        public List<(double, double)> Measurements
        {
            get 
            {
                _measurements = _measurements.OrderBy(x => x.Item1).ToList();
                return _measurements;
            }
            set 
            { 
                _measurements = value;
            }
        }
            

        public TankTableModel()
        {
            Measurements = new List<(double, double)>();
        }
    }
}
