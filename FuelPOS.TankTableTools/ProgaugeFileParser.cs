using FuelPOS.TankTableTools.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public class ProgaugeFileParser : IProgaugeFileParser
    {
        private string _folderPath;
        private Dictionary<string, List<string>> _tableFiles = new();
        private bool _isFirstTank = true;
        private bool _tankNumZeroIndexed = false;
        private readonly ILogger<ProgaugeFileParser> _logger;

        public List<TankTableModel> TankTables { get; private set; } = new();

        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; }
        }

        public ProgaugeFileParser(ILogger<ProgaugeFileParser> logger)
        {
            _logger = logger;
        }

        public void LoadFilesAndParse()
        {
            var files = Directory.EnumerateFiles(FolderPath, "*.csv").ToList();
            files.AddRange(Directory.EnumerateFiles(FolderPath, ".txt").ToList());
            
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                string tankNumber = GetTankNumber(fileName);

                if (tankNumber.Length > 0)
                {
                    _tableFiles.Add(tankNumber, File.ReadAllLines(file).ToList());
                }
                else
                {
                    throw new Exception($"Tank number could not be found in file name: {fileName}\n" +
                        $"Ensure file name contains the tank number.");
                }
            }

            ParseFiles();
            CreateTankTables();
        }

        public void ParseFiles()
        {
            foreach (var file in _tableFiles)
            {
                List<string> newValue = new();

                foreach (var line in file.Value)
                {
                    var newLine = ParseLine(line);

                    if (newLine is not null)
                        newValue.Add(newLine);
                }

                _tableFiles[file.Key] = newValue;
            }
        }

        private string ParseLine(string line)
        {
            if (line.Any(char.IsLetter))
            {
                return null;
            }

            var newLine = line.ToSemiColonSeparated();

            var split = newLine.Split(';');

            newLine = $"{split[0]};{split[1]}";

            return newLine;
        }

        private string GetTankNumber(string fileName)
        {
            string tankNumber = string.Join("", fileName.Where(char.IsDigit));

            if (_isFirstTank)
            {
                if (tankNumber.Length > 1)
                {
                    _tankNumZeroIndexed = true;
                }

                _isFirstTank = false;
            }

            if (_tankNumZeroIndexed)
            {
                var tankInt = int.Parse(tankNumber) + 1;
                tankNumber = tankInt.ToString();
            }

            return tankNumber;
        }

        private void CreateTankTables()
        {
            foreach (var tank in _tableFiles)
            {
                TankTableModel table = new();

                table.TankNumber = tank.Key;

                foreach (var line in tank.Value)
                {
                    List<string> newLine = line.Split(';').ToList();

                    if (newLine.Count > 1)
                        table.CreateValuePairs(newLine);
                }

                TankTables.Add(table);
            }
        }
    }
}
