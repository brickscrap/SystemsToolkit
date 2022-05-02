using DocumentFormat.OpenXml.Spreadsheet;
using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SpreadsheetLight;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SysTk.Utils.Spreadsheets
{
    internal class FuelPosSurveySheet : ISpreadsheet<StatdevModel>
    {
        private readonly ILogger _logger;

        private static SLStyle ColumnHeaderStyle
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

        private static SLStyle TitleStyle
        {
            get
            {
                var style = new SLStyle();
                style.Font.Bold = true;
                style.Font.FontSize = 14;

                return style;
            }
        }

        private static SLStyle SubTitleStyle
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

        public string Name { get { return "FuelPOS Survey"; } }

        public FuelPosSurveySheet(ILogger logger = null)
        {
            _logger = logger ?? NullLogger.Instance;
        }

         // TODO: Make this monstrosity maintainable
        public SLDocument Create(IEnumerable<StatdevModel> data)
        {
            SLDocument doc = new();

            doc.AddWorksheet("Summary");
            doc = SetupSummary(doc, data);

            int rowNumber = 3;
            foreach (var station in data)
            {
                doc = PopulateSummary(doc, station, rowNumber);
                rowNumber++;
            }

            doc.AutoFitColumn(1, 71);

            var table = doc.CreateTable(2, 1, data.Count() + 3, 74);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            doc.InsertTable(table);

            foreach (var station in data)
            {
                _logger.LogInformation("Creating worksheet for {Station}", station.StationInfo.StationName);
                if (doc.AddWorksheet(SanitiseStationName(station.StationInfo.StationName)))
                {
                    doc = SetupTemplate(doc);

                    doc.MergeWorksheetCells("A1", "C1");
                    doc.SetCellValue("A1", station.StationInfo.StationName);
                    doc.SetCellStyle("A1", TitleStyle);
                    doc.SetCellValue("A2", "PSE ID:");
                    doc.SetCellStyle("A2", TitleStyle);
                    doc.SetCellValue("B2", station.StationInfo.StationNumber);
                    doc.SetCellStyle("B2", TitleStyle);

                    doc = AddPos(doc, station);
                    doc = AddTankManagement(doc, station);
                    doc = AddDispensers(doc, station);
                };
            }

            doc.DeleteWorksheet("Sheet1");

            return doc;
        }

        private SLDocument SetupSummary(SLDocument doc, IEnumerable<StatdevModel> data)
        {
            // Titles
            doc.MergeWorksheetCells("A1", "C1");
            doc.MergeWorksheetCells("D1", "F1");
            doc.SetCellValue("D1", "CIS");
            doc.SetCellStyle("D1", SubTitleStyle);
            doc.MergeWorksheetCells("G1", "S1");
            doc.SetCellValue("G1", "POS 1/INT");
            doc.SetCellStyle("G1", SubTitleStyle);
            doc.MergeWorksheetCells("T1", "AF1");
            doc.SetCellValue("T1","POS 2");
            doc.SetCellStyle("T1", SubTitleStyle);
            doc.MergeWorksheetCells("AG1", "AS1");
            doc.SetCellValue("AG1", "POS 3");
            doc.SetCellStyle("AG1", SubTitleStyle);
            doc.MergeWorksheetCells("AT1", "BF1");
            doc.SetCellValue("AT1", "POS 4");
            doc.SetCellStyle("AT1", SubTitleStyle);
            doc.MergeWorksheetCells("BG1", "BS1");
            doc.SetCellValue("BG1", "POS 5");
            doc.SetCellStyle("BG1", SubTitleStyle);
            doc.MergeWorksheetCells("BT1", "BV1");
            doc.SetCellValue("BT1", "Protocols");
            doc.SetCellStyle("BT1", SubTitleStyle);

            // Table headers
            doc.SetCellValue("A2", "PSE ID");
            doc.SetCellValue("B2", "Station Name");
            doc.SetCellValue("C2", "Nr of POS");

            // CIS
            doc.SetCellValue("D2", "Hardware");
            doc.SetCellValue("E2", "Build Disk");
            doc.SetCellValue("F2", "UPS");

            // POS 1
            doc.SetCellValue("G2", "Hardware");
            doc.SetCellValue("H2", "Build Disk");
            doc.SetCellValue("I2", "Touchscreen");
            doc.SetCellValue("J2", "UPS");
            doc.SetCellValue("K2", "Scanner");
            doc.SetCellValue("L2", "Printer");
            doc.SetCellValue("M2", "CDU");
            doc.SetCellValue("N2", "MSR");
            doc.SetCellValue("O2", "OASE IPT");
            doc.SetCellValue("P2", "Ext IPT");
            doc.SetCellValue("Q2", "Ext Loyalty");
            doc.SetCellValue("R2", "Nr Serial Devices");
            doc.SetCellValue("S2", "Nr Multiport Devices");

            // POS 2
            doc.SetCellValue("T2", "Hardware");
            doc.SetCellValue("U2", "Build Disk");
            doc.SetCellValue("V2", "Touchscreen");
            doc.SetCellValue("W2", "UPS");
            doc.SetCellValue("X2", "Scanner");
            doc.SetCellValue("Y2", "Printer");
            doc.SetCellValue("Z2", "CDU");
            doc.SetCellValue("AA2", "MSR");
            doc.SetCellValue("AB2", "OASE IPT");
            doc.SetCellValue("AC2", "Ext IPT");
            doc.SetCellValue("AD2", "Ext Loyalty");
            doc.SetCellValue("AE2", "Nr Serial Devices");
            doc.SetCellValue("AF2", "Nr Multiport Devices");

            // POS 3
            doc.SetCellValue("AG2", "Hardware");
            doc.SetCellValue("AH2", "Build Disk");
            doc.SetCellValue("AI2", "Touchscreen");
            doc.SetCellValue("AJ2", "UPS");
            doc.SetCellValue("AK2", "Scanner");
            doc.SetCellValue("AL2", "Printer");
            doc.SetCellValue("AM2", "CDU");
            doc.SetCellValue("AN2", "MSR");
            doc.SetCellValue("AO2", "OASE IPT");
            doc.SetCellValue("AP2", "Ext IPT");
            doc.SetCellValue("AQ2", "Ext Loyalty");
            doc.SetCellValue("AR2", "Nr Serial Devices");
            doc.SetCellValue("AS2", "Nr Multiport Devices");

            // POS 4
            doc.SetCellValue("AT2", "Hardware");
            doc.SetCellValue("AU2", "Build Disk");
            doc.SetCellValue("AV2", "Touchscreen");
            doc.SetCellValue("AW2", "UPS");
            doc.SetCellValue("AX2", "Scanner");
            doc.SetCellValue("AY2", "Printer");
            doc.SetCellValue("AZ2", "CDU");
            doc.SetCellValue("BA2", "MSR");
            doc.SetCellValue("BB2", "OASE IPT");
            doc.SetCellValue("BC2", "Ext IPT");
            doc.SetCellValue("BD2", "Ext Loyalty");
            doc.SetCellValue("BE2", "Nr Serial Devices");
            doc.SetCellValue("BF2", "Nr Multiport Devices");

            // POS 5
            doc.SetCellValue("BG2", "Hardware");
            doc.SetCellValue("BH2", "Build Disk");
            doc.SetCellValue("BI2", "Touchscreen");
            doc.SetCellValue("BJ2", "UPS");
            doc.SetCellValue("BK2", "Scanner");
            doc.SetCellValue("BL2", "Printer");
            doc.SetCellValue("BM2", "CDU");
            doc.SetCellValue("BN2", "MSR");
            doc.SetCellValue("BO2", "OASE IPT");
            doc.SetCellValue("BP2", "Ext IPT");
            doc.SetCellValue("BQ2", "Ext Loyalty");
            doc.SetCellValue("BR2", "Nr Serial Devices");
            doc.SetCellValue("BS2", "Nr Multiport Devices");
        
            // Protocols
            doc.SetCellValue("BT2", "Proto 1");
            doc.SetCellValue("BU2", "Proto 2");
            doc.SetCellValue("BV2", "Proto 3");

            return doc;
        }

        private SLDocument PopulateSummary(SLDocument doc, StatdevModel station, int row)
        {
            doc.SetCellValue($"A{row}", station.StationInfo.StationNumber);
            doc.SetCellValue($"B{row}", station.StationInfo.StationName);
            doc.SetCellValue($"C{row}", station.StationInfo.NumberOfPos);

            int col = 4;
            // Add POS
            foreach (var pos in station.POS)
            {
                if (pos.Number == 9)
                {
                    doc.SetCellValue(row, 4, pos.HardwareType);
                    doc.SetCellValue(row, 5, pos.BuildDisk);
                    doc.SetCellValue(row, 6, pos.UPS);
                    continue;
                }

                if (pos.Number == 1)
                    col = 7;

                doc.SetCellValue(row, col++, pos.HardwareType);
                doc.SetCellValue(row, col++, pos.BuildDisk);
                doc.SetCellValue(row, col++, pos.TouchScreenType);
                doc.SetCellValue(row, col++, pos.UPS);
                doc.SetCellValue(row, col++, pos.BarcodeScanner);
                doc.SetCellValue(row, col++, pos.ReceiptPrinter);
                doc.SetCellValue(row, col++, pos.CustomerDisplay);
                doc.SetCellValue(row, col++, pos.MagCardReader);
                doc.SetCellValue(row, col++, pos.PinPad.Name);
                doc.SetCellValue(row, col++, pos.PaymentTerminal);
                doc.SetCellValue(row, col++, pos.LoyaltyTerminal);
                doc.SetCellValue(row, col++, pos.ComDevices.NumberSerialPortsInUse);
                doc.SetCellValue(row, col++, pos.ComDevices.NumberPciMultiPortsInUse);
            }

            col = 72;
            // Add Protocols
            foreach (var proto in station.POS.Select(x => x.DispenserCommTypes))
            {
                foreach (var item in proto)
                {
                    doc.SetCellValue(row, col, item);
                    col++;
                }
            }

            return doc;
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
            doc.SetCellValue("B2", data.StationInfo.StationNumber);

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
