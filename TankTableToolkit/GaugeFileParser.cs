using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using TankTableToolkit.Models;

namespace TankTableToolkit
{
    public class GaugeFileParser
    {
        private string _filePath;
        private List<string> _serialisedFile;
        private string _siteName;
        private string _currentTank;
        private string _nextTank;

        private List<TankTableModel> _tankTables = new List<TankTableModel>();

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
        public List<TankTableModel> Parse()
        {
            LoadFile();
            _nextTank = _serialisedFile.Where(x => x.Contains("TANK")).First();

            CreateTankTables();

            return _tankTables;
        }

        public void DisplayTablesInConsole()
        {
            foreach (var item in _tankTables)
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
            var file = File.ReadAllLines(_filePath);
            _serialisedFile = SerialiseGaugeFile(file);
        }

        private List<string> SerialiseGaugeFile(string[] file)
        {
            List<string> output = new List<string>();
            var regex = new Regex("   +");

            foreach (var line in file)
            {
                var parsedLine = ParseLine(line);
                if (parsedLine != null)
                {
                    output.Add(parsedLine);
                }
            }

            return output;
        }

        private string ParseLine(string line)
        {
            if (line.Contains("TANK CALIBRATION") || line.Contains("--"))
            {
                return null;
            }

            var regex = new Regex("   +");
            var newLine = line.Trim();
            newLine = regex.Replace(newLine, ";").Trim();

            if (newLine.Contains("TANK"))
            {
                var sub = newLine.IndexOf("TANK");
                return newLine.Substring(sub);
            }

            if (newLine.Any(Char.IsDigit) && !newLine.Any(Char.IsLetter))
            {
                return newLine;
            }

            return null;
        }

        private TankTableModel CreateValuePairs(TankTableModel tankTable, List<string> line)
        {
            for (int i = 0; i < line.Count; i = i + 2)
            {
                double mm = double.Parse(line[i].Trim());
                double litres = double.Parse(line[i + 1].Trim());
                tankTable.Measurements.Add((mm, litres));
            }

            return tankTable;
        }

        private string GetTankNumber(string input)
        {
            char[] matchOn = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            var subString = input.IndexOfAny(matchOn);

            if (subString != -1)
            {
                var output = input.Insert(subString, ",")
                .Split(',')[1];
                return output;
            }

            return null;
        }

        private void CreateTankTables()
        {
            List<TankTableModel> output = new List<TankTableModel>();
            TankTableModel table = new TankTableModel();

            foreach (var line in _serialisedFile)
            {
                var newLine = line.Split(';').ToList();

                if (newLine.Any(x => x.Contains("TANK")))
                {
                    _currentTank = newLine.First();

                    if (_nextTank == _currentTank)
                    {
                        table.TankNumber = GetTankNumber(_currentTank);
                    }
                    else
                    {
                        _tankTables.Add(table);
                        _nextTank = _currentTank;
                        table = new TankTableModel();
                        table.TankNumber = GetTankNumber(_currentTank);
                    }

                    continue;
                }

                CreateValuePairs(table, newLine);
            }

            // Add the last tank
            _tankTables.Add(table);
        }
    }
}
