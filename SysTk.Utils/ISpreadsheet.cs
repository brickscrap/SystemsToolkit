using FuelPOS.StatDevParser.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.Utils
{
    public interface ISpreadsheet<T>
    {
        public string Name { get; }
        public SLDocument Create(IEnumerable<T> data);
    }
}
