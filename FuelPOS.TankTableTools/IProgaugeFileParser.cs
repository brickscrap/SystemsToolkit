using System.Collections.Generic;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public interface IProgaugeFileParser
    {
        string FolderPath { get; set; }
        List<TankTableModel> TankTables { get; }

        void LoadFilesAndParse();
        void ParseFiles();
    }
}