using System.Collections.Generic;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public interface IVdrRootFileParser
    {
        string FilePath { get; set; }
        string SiteName { get; }
        List<TankTableModel> TankTables { get; }

        void Parse();
    }
}