using System.Collections.Generic;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public class BasicFileParser
    {
        private string _folderPath;
        private List<string> _file;
        private List<TankTableModel> _tankTables;

        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; }
        }

        public List<TankTableModel> TankTables
        {
            get { return _tankTables; }
            set { _tankTables = value; }
        }

        public BasicFileParser(string folderPath)
        {
            FolderPath = folderPath;
        }

        public void Parse()
        {

        }
    }
}
