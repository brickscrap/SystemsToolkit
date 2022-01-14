using FuelPOS.TankTableTools.Helpers;
using FuelPOS.TankTableTools.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public class VdrRootFileParser : IVdrRootFileParser
    {
        private readonly static List<string> _possibleSections = new()
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
        private readonly ILogger<VdrRootFileParser> _logger;
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

        public VdrRootFileParser(ILogger<VdrRootFileParser> logger = null)
        {
            _logger = logger ?? NullLogger<VdrRootFileParser>.Instance;
        }

        /// <summary>
        /// Create a list of tank tables based on the file provided to the constructor
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="TankTableModel"/></returns>
        public void Parse()
        {
            _logger.LogInformation("Parsing {FilePath}", FilePath);
            _file = LoadFile();

            ParseSections();
            _logger.LogDebug("Sections detected: {Sections}", _sections.Select(x => x.Key));

            var i21100Section = _sections.Where(x => x.Key == "I21100")
                .ToList()
                .FirstOrDefault()
                .Value;
            _siteName = FindSiteName(i21100Section);
            _logger.LogInformation("Site name detected as {SiteName}", _siteName);


            foreach (var line in _sections)
                Serialise(line);
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

        private List<string> LoadFile() => File.ReadAllLines(_filePath).ToList();

        private string FindSiteName(List<string> i2110Section)
        {
            List<string> stripped = new();
            for (int i = 0; i < 10; i++)
            {
                if (!string.IsNullOrWhiteSpace(i2110Section[i]))
                {
                    stripped.Add(i2110Section[i]);
                }
            }

            var siteName = stripped[2].Split("   ");

            return siteName[0];
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
                    _logger.LogInformation("Parsing calibration chart.");
                    CalChartHandler calChart = new();
                    TankTables = calChart.Parse(kvp.Value);
                    _logger.LogInformation("{Tanks} tanks detected.", TankTables.Count);
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
