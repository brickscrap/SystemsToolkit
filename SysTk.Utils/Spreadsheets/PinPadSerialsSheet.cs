using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.Utils.Spreadsheets
{
    internal class PinPadSerialsSheet : ISpreadsheet<List<StatdevModel>>
    {
        private readonly ILogger _logger;

        public PinPadSerialsSheet(ILogger logger = null)
        {
            _logger = logger ?? NullLogger.Instance;
        }

        public SLDocument Create(List<StatdevModel> data) 
        {
            SLDocument doc = new();

            doc = SetupTemplate(doc);

            int row = 2;

            foreach (var station in data)
            {
                foreach (var pos in station.POS)
                {
                    if (!string.IsNullOrWhiteSpace(pos.PinPad.Name))
                    {
                        doc.SetCellValue(row, 1, station.StationInfo.StationNumber);
                        doc.SetCellValue(row, 2, station.StationInfo.StationName);
                        doc.SetCellValue(row, 3, pos.PinPad.Name);
                        doc.SetCellValue(row, 4, pos.PinPad.SerialNumber);

                        row++;
                    }

                    foreach (var opt in pos.OutdoorTerminals)
                    {
                        doc.SetCellValue(row, 1, station.StationInfo.StationNumber);
                        doc.SetCellValue(row, 2, station.StationInfo.StationName);
                        doc.SetCellValue(row, 3, opt.HardwareType);
                        doc.SetCellValue(row, 4, opt.PinPad.SerialNumber);

                        row++;
                    }
                }
            }

            doc.AutoFitColumn(1);
            doc.AutoFitColumn(2);
            doc.AutoFitColumn(3);
            doc.AutoFitColumn(4);

            var table = doc.CreateTable(1, 1, row, 4);
            doc.InsertTable(table);

            return doc;
        }

        private SLDocument SetupTemplate(SLDocument doc)
        {
            doc.SetCellValue("A1", "PSE ID");
            doc.SetCellValue("B1", "Station");
            doc.SetCellValue("C1", "Device");
            doc.SetCellValue("D1", "Serial Number");

            doc.AutoFitRow(1);

            return doc;
        }
    }
}
