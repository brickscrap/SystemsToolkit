using DocumentFormat.OpenXml.Spreadsheet;
using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SpreadsheetLight;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SysTk.Utils
{
    public class SpreadsheetCreator
    {
        private readonly ILogger _logger;

        private SLStyle ColumnHeaderStyle
        {
            get
            {
                var style = new SLStyle();
                style.Font.Bold = true;
                style.Alignment = new SLAlignment { Horizontal = HorizontalAlignmentValues.Left };
                System.Drawing.Color lightSteelBlue = System.Drawing.Color.LightSteelBlue;
                style.SetPatternFill(PatternValues.Solid, lightSteelBlue, lightSteelBlue);

                return style;
            }
        }

        private SLStyle TitleStyle
        {
            get
            {
                var style = new SLStyle();
                style.Font.Bold = true;
                style.Font.FontSize = 14;

                return style;
            }
        }

        private SLStyle SubTitleStyle
        {
            get
            {
                var style = new SLStyle();
                style.Font.Bold = true;
                style.Font.FontSize = 14;
                style.Alignment = new SLAlignment { Horizontal = HorizontalAlignmentValues.Center };

                return style;
            }
        }

        public SpreadsheetCreator(ILogger logger = null)
        {
            _logger = logger ?? NullLogger.Instance;
        }

        // TODO: Make this monstrosity maintainable
        public void CreateFuelPosSurvey(List<StatdevModel> data, string outputPath)
        {
            SLDocument doc = new();

            foreach (var station in data)
            {
                _logger.LogInformation("Creating worksheet for {Station}", station.StationInfo.StationName);
                if (doc.AddWorksheet(SanitiseStationName(station.StationInfo.StationName)))
                {
                    doc = SetupTemplate(doc);

                    doc.MergeWorksheetCells("A1", "C1");
                    doc.SetCellValue("A1", station.StationInfo.StationName);
                    doc.SetCellStyle("A1", TitleStyle);

                    doc = AddPos(doc, station);
                    doc = AddTankManagement(doc, station);
                    doc = AddDispensers(doc, station);
                };
            }

            doc.DeleteWorksheet("Sheet1");

            string filePath = $@"{outputPath}\FuelPOS Survey.xlsx";
            _logger.LogInformation("Saving to {OutputPath}", filePath);
            doc.SaveAs(filePath);
        }

        private SLDocument SetupTemplate(SLDocument doc)
        {
            // POS Template
            doc.SetCellValue("A3", "POS Details");
            doc.MergeWorksheetCells("A3", "C3");

            doc.SetCellValue("A5", "Hardware");
            doc.SetCellValue("A6", "Build Disk");
            doc.SetCellValue("A7", "OS");
            doc.SetCellValue("A8", "Touchscreen");
            doc.SetCellValue("A9", "UPS");
            doc.SetCellValue("A10", "Scanner");
            doc.SetCellValue("A11", "Printer");
            doc.SetCellValue("A12", "CDU");
            doc.SetCellValue("A13", "MSR");
            doc.SetCellValue("A14", "OASE IPT");
            doc.SetCellValue("A15", "Ext IPT");
            doc.SetCellValue("A16", "Ext Loyalty");
            doc.SetCellValue("A17", "Nr Ser Dev");
            doc.SetCellValue("A18", "Nr Multiport Dev");
            doc.SetCellValue("A19", "LON Interface");
            doc.SetCellValue("A20", "A4 Printer");

            // Tank Management Template
            doc.SetCellValue("A21", "Tank Management");
            doc.MergeWorksheetCells("A21", "C21");

            doc.SetCellValue("A26", "Fuel");
            doc.SetCellValue("A27", "Fuel Number");

            // Dispenser Template
            doc.SetCellValue("A29", "Dispensers");
            doc.MergeWorksheetCells("A29", "C29");

            doc.SetCellValue("A31", "Comms Types");

            // Styles
            for (int i = 5; i < 21; i++)
            {
                doc.SetCellStyle(i, 1, ColumnHeaderStyle);
            }
            doc.SetCellStyle("A3", SubTitleStyle);
            doc.SetCellStyle("A21", SubTitleStyle);
            doc.SetCellStyle("A29", SubTitleStyle);
            doc.SetCellStyle("A31", ColumnHeaderStyle);

            // Layout
            doc.AutoFitColumn("A");

            return doc;
        }

        private SLDocument AddPos(SLDocument doc, StatdevModel data)
        {
            for (int i = 0; i < data.POS.Count; i++)
            {
                int j = i + 2;
                doc = SetPOSHeaders(doc, data, i, j, 4);

                doc.SetCellValue(5, j, data.POS[i].HardwareType);
                doc.SetCellValue(6, j, data.POS[i].BuildDisk);
                doc.SetCellValue(7, j, data.POS[i].OperatingSystem);
                doc.SetCellValue(8, j, data.POS[i].TouchScreenType);
                doc.SetCellValue(9, j, data.POS[i].UPS);
                doc.SetCellValue(10, j, data.POS[i].BarcodeScanner);
                doc.SetCellValue(11, j, data.POS[i].ReceiptPrinter);
                doc.SetCellValue(12, j, data.POS[i].CustomerDisplay);
                doc.SetCellValue(13, j, data.POS[i].MagCardReader);
                doc.SetCellValue(14, j, data.POS[i].PinPad.Name);
                doc.SetCellValue(15, j, data.POS[i].PaymentTerminal);
                doc.SetCellValue(16, j, data.POS[i].LoyaltyTerminal);
                doc.SetCellValue(17, j, data.POS[i].ComDevices.NumberSerialPortsInUse);
                doc.SetCellValue(18, j, data.POS[i].ComDevices.NumberPciMultiPortsInUse);
                doc.SetCellValue(19, j, data.POS[i].ComDevices.LonInterface);

                doc.AutoFitColumn(j);
            }

            var table = doc.CreateTable(4, 2, 18, data.POS.Count + 1);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            doc.InsertTable(table);
            doc.SetCellValue("B20", data.POS[0].A4Printer);


            return doc;
        }
        private SLDocument AddTankManagement(SLDocument doc, StatdevModel data)
        {
            doc.SetCellValue("A23", "Level Gauge");
            doc.SetCellValue("B23", data.TankManagement.TankGauge);

            for (int i = 0; i < data.TankManagement.TankGroups.Count; i++)
            {
                int j = i + 2;

                doc.SetCellValue(25, j, $"Tank Group {data.TankManagement.TankGroups[i].Number}");

                doc.SetCellValue(26, j, data.TankManagement.TankGroups[i].ProductName);
                doc.SetCellValue(27, j, data.TankManagement.TankGroups[i].ProductNumber);

                doc.AutoFitColumn(j);
            }

            doc.SetCellStyle("A23", ColumnHeaderStyle);

            for (int i = 26; i < 28; i++)
            {
                doc.SetCellStyle(i, 1, ColumnHeaderStyle);
            }

            var table = doc.CreateTable(25, 2, 27, data.TankManagement.TankGroups.Count + 1);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            doc.InsertTable(table);

            return doc;
        }

        private SLDocument AddDispensers(SLDocument doc, StatdevModel data)
        {
            for (int i = 0; i < data.POS.Count; i++)
            {
                int j = i + 2;

                doc = SetPOSHeaders(doc, data, i, j, 30);

                string comms = "";
                foreach (var item in data.POS[i].DispenserCommTypes)
                {
                    comms += $"{item}, ";
                }

                comms = comms.Trim().Trim(',');

                doc.SetCellValue(31, j, comms);
            }

            var table = doc.CreateTable(30, 2, 31, data.POS.Count + 1);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            doc.InsertTable(table);

            return doc;
        }

        private static string SanitiseStationName(string stationName)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(stationName, "");
        }

        private static SLDocument SetPOSHeaders(SLDocument doc, StatdevModel data, int i, int j, int row)
        {
            if (data.POS[i].Number == 9)
                doc.SetCellValue(row, j, $"CIS");
            else
                doc.SetCellValue(row, j, $"POS {data.POS[i].Number}");

            return doc;
        }
    }
}
