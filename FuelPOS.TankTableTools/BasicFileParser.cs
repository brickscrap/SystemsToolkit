using FuelPOS.TankTableTools.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TankTableToolkit.Models;

namespace FuelPOS.TankTableTools
{
    public class BasicFileParser
    {
        private string _folderPath;
        private Dictionary<string, List<string>> _tableFiles = new();

        public List<TankTableModel> TankTables { get; private set; } = new();

        public string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; }
        }

        public BasicFileParser(string folderPath)
        {
            FolderPath = folderPath;
        }

        public void LoadFilesAndParse()
        {
            foreach (var file in Directory.EnumerateFiles(FolderPath))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                string tankNumber = string.Join("", fileName.Where(char.IsDigit));

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
                    {
                        newValue.Add(newLine);
                    }
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
                    {
                        table.CreateValuePairs(newLine);
                    }
                }

                TankTables.Add(table);
            }
        }
    }
}
