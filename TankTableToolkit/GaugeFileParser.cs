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
        private List<string> _file;
        private string _site;
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
        }
        
        /// <summary>
        /// Create a list of tank tables based on the file provided to the constructor
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="TankTableModel"/></returns>
        public List<TankTableModel> Parse()
        {
            LoadFile();
            _site = _file[0].Split(';')[0];
            _nextTank = _file[0].Split(';')[1];

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
            _file = RemoveExcessData(file);
        }
        private List<string> RemoveExcessData(string[] file)
        {
            List<string> output = new List<string>();
            var regex = new Regex("  +");

            foreach (var line in file)
            {
                var newLine = regex.Replace(line, ";").Trim();
                if (!string.IsNullOrWhiteSpace(newLine))
                {
                    if (char.IsDigit(newLine[0]))
                    {
                        output.Add(regex.Replace(line, ";"));
                    }
                }
            }

            return output;
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

            foreach (var line in _file)
            {
                var newLine = line.Split(';').ToList();

                // If this line is the tank number indicator
                if (newLine[0] == _site)
                {
                    _currentTank = newLine[1];
                    if (_nextTank == _currentTank)
                    {
                        table.TankNumber = GetTankNumber(_currentTank);
                        continue;
                    }
                    else
                    {
                        _tankTables.Add(table);
                        _nextTank = _currentTank;
                        table = new TankTableModel();
                        table.TankNumber = GetTankNumber(_currentTank);
                    }
                }
                else
                {
                    CreateValuePairs(table, newLine);
                }
            }

            // Add the last tank
            _tankTables.Add(table);
        }
    }
}
