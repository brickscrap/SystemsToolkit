using FuelPOS.TankTableTools.Helpers;
using FuelPOS.TankTableTools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    // TODO: Handle individual cal file
    public class GaugeFileParser
    {
        private readonly static List<string> _possibleSections = new List<string>
        {
                "I20100",
                "I60100",
                "I60200",
                "I60700",
                "I62800",
                "I60600",
                "I79S00",
                "I21100",
                "I61L00",
                "I62100",
                "I62500"
        };

        private bool _isFullFileFlag;
        private string _filePath;
        private List<string> _file;
        private Dictionary<string, List<string>> _sections = new();
        private string _siteName;

        public string SiteName
        {
            get { return _siteName; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }


        public List<TankTableModel> TankTables { get; private set; }
        public List<MaxVolModel> MaxVols { get; set; }
        public List<CsvOutputModel> CsvOutput
        {
            get
            {
                return CsvOutputCreator.Create(TankTables, MaxVols);
            }
        }

        public GaugeFileParser()
        {

        }

        /// <summary>
        /// Reads and converts a tank table file from a VeederRoot
        /// </summary>
        /// <param name="filePath">The full path to the table file</param>
        public GaugeFileParser(string filePath)
        {
            _filePath = filePath;
            _siteName = Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Create a list of tank tables based on the file provided to the constructor
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="TankTableModel"/></returns>
        public void Parse(bool isFullFile = false)
        {
            _isFullFileFlag = isFullFile;
            LoadFile();
            ParseSections();

            foreach (var item in _sections)
            {
                Serialise(item);
            }
        }

        public void DisplayTablesInConsole()
        {
            foreach (var item in TankTables)
            {
                Console.WriteLine(item.TankNumber);
                foreach (var measurement in item.Measurements)
                {
                    Console.WriteLine($"{measurement.Item1}, {measurement.Item2}");
                }
            }
        }

        private void LoadFile()
        {
            _file = File.ReadAllLines(_filePath).ToList();
            if (_isFullFileFlag)
            {
                if (_file[1].Contains("Site Count"))
                {
                    _siteName = _file[2];
                }
                else
                {
                    _siteName = _file[1];
                }

                if (SiteName.Contains("450+"))
                {
                    var position = _siteName.IndexOf("450");
                    _siteName = _siteName.Substring(0, position).Trim();
                }
            }
        }

        private void ParseSections()
        {
            List<KeyValuePair<string, int>> sectionPositions = new();

            foreach (var section in _possibleSections)
            {
                sectionPositions = GetSectionPosition(sectionPositions, section);
            }

            sectionPositions = sectionPositions.OrderBy(x => x.Value).ToList();

            Dictionary<string, List<string>> outputSections = new();

            foreach (var sec in sectionPositions)
            {
                outputSections[sec.Key] = GetSection(sec.Key, sectionPositions);
            }

            _sections = outputSections;
        }

        private List<KeyValuePair<string, int>> GetSectionPosition(List<KeyValuePair<string, int>> list, string key)
        {
            if (_file.IndexOf(key) > -1)
            {
                list.Add(new KeyValuePair<string, int>(key, _file.IndexOf(key)));
            }
            
            return list;
        }

        private List<string> GetSection(string key, List<KeyValuePair<string, int>> sectionPositions)
        {
            var sectionStart = sectionPositions.Where(
                x => x.Key == key).FirstOrDefault().Value;

            var count = sectionPositions.SkipWhile(x => x.Key != key)
                .Skip(1)
                .DefaultIfEmpty(sectionPositions.Last())
                .FirstOrDefault()
                .Value - sectionStart;

            if (count == 0)
            {
                count = _file.Count() - sectionStart;
            }

            return _file.GetRange(sectionStart, count - 1);
        }

        private void Serialise(KeyValuePair<string, List<string>> kvp)
        {
            switch (kvp.Key)
            {
                case "I21100":
                    CalChartHandler calChart = new(_isFullFileFlag);
                    TankTables = calChart.Parse(kvp.Value);
                    break;
                case "I62800":
                    MaxVolHandler maxVol = new();
                    MaxVols = maxVol.Parse(kvp.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
