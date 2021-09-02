using FuelPOS.TankTableTools.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    internal class CalChartHandler
    {
        private bool _isFullFile;
        private string _siteName;
        private string _currentTank;
        private string _nextTank;
        private List<string> _serialisedCalChart = new();
        private List<TankTableModel> _tankTables = new List<TankTableModel>();

        public CalChartHandler(bool isFullFile)
        {
            _isFullFile = isFullFile;
        }

        public List<TankTableModel> Parse(List<string> tankCalChart)
        {
            List<string> serialisedChart = new();
            foreach (var line in tankCalChart)
            {
                var parsedLine = ParseLine(line);
                if (parsedLine is not null)
                {
                    serialisedChart.Add(parsedLine);
                }
            }

            _serialisedCalChart = serialisedChart;

            CreateTankTables();

            return _tankTables;
        }

        private string ParseLine(string line)
        {
            if (line.Contains("TANK CALIBRATION") || line.Contains("--") || line.Contains('/') || line.Contains(':'))
            {
                return null;
            }

            var newLine = line.ToSemiColonSeparated();

            if (newLine.Contains("TANK"))
            {
                var sub = newLine.IndexOf("TANK");
                return newLine.Substring(sub);
            }

            if (newLine.Any(char.IsDigit) && !newLine.Any(char.IsLetter))
            {
                if (!newLine.Contains('/') || !newLine.Contains(':'))
                {
                    return newLine;
                }
            }

            return null;
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
            List<TankTableModel> output = new();
            TankTableModel table = new();

            _nextTank = _serialisedCalChart.Where(x => x.Contains("TANK")).FirstOrDefault();

            foreach (var line in _serialisedCalChart)
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

                table.CreateValuePairs(newLine);
            }

            // Add the last tank
            _tankTables.Add(table);
        }
    }
}
