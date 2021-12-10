using FuelPOS.StatDevParser.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace SysTk.Utils
{
    public class SpreadsheetCreator
    {
        // TODO: Formatting
        // TODO: Split into separate methods
        public static void CreateFuelPosSurvey(List<StatdevModel> data, string outputPath)
        {
            SLDocument sheet = new();

            // Set up header style
            var headerStyle = sheet.CreateStyle();
            var headerAlign = new SLAlignment();
            headerAlign.Horizontal = HorizontalAlignmentValues.Center;
            headerStyle.Alignment = headerAlign;
            headerStyle.Font.Bold = true;

            // Set up headers, first row
            sheet.MergeWorksheetCells("A1", "B1");
            sheet.MergeWorksheetCells("C1", "I1");
            sheet.SetCellValue("C1", "POS 1");
            sheet.MergeWorksheetCells("J1", "O1");
            sheet.SetCellValue("J1", "POS 2");
            sheet.MergeWorksheetCells("P1", "U1");
            sheet.SetCellValue("P1", "POS 3");

            // Headers, second row
            sheet.SetCellValue("A2", "Petrol Server");
            sheet.SetCellValue("B2", "Site Name");
            sheet.SetCellValue("C2", "Hardware");
            sheet.SetCellValue("D2", "BD");
            sheet.SetCellValue("E2", "SW Ver");
            sheet.SetCellValue("F2", "CDU");
            sheet.SetCellValue("G2", "Scanner");
            sheet.SetCellValue("H2", "UPS");
            sheet.SetCellValue("I2", "Ser Ports Used");
            sheet.SetCellValue("J2", "Hardware 2");
            sheet.SetCellValue("K2", "BD 2");
            sheet.SetCellValue("L2", "CDU 2");
            sheet.SetCellValue("M2", "Scanner 2");
            sheet.SetCellValue("N2", "UPS 2");
            sheet.SetCellValue("O2", "Ser Ports Used 2");
            sheet.SetCellValue("P2", "Hardware 3");
            sheet.SetCellValue("Q2", "BD 3");
            sheet.SetCellValue("R2", "CDU 3");
            sheet.SetCellValue("S2", "Scanner 3");
            sheet.SetCellValue("T2", "UPS 3");
            sheet.SetCellValue("U2", "Ser Ports Used 3");
            sheet.SetCellValue("V2", "Protocol 1");
            sheet.SetCellValue("W2", "Protocol 2");
            sheet.SetCellValue("X2", "Protocol 3");

            // Apply header style
            sheet.SetRowStyle(1, headerStyle);
            sheet.SetRowStyle(2, headerStyle);

            // Add data
            int r = 3;

            foreach (var value in data)
            {
                sheet.SetCellValue(r, 1, value.StationInfo.StationNumber);
                sheet.SetCellValue(r, 2, value.StationInfo.StationName);

                int c = 3;

                foreach (var pos in value.POS)
                {
                    sheet.SetCellValue(r, c, pos.HardwareType);
                    sheet.SetCellValue(r, c + 1, pos.OperatingSystem);

                    if (pos.Number == 1)
                    {
                        sheet.SetCellValue(r, c + 2, pos.SoftwareVersion);
                        sheet.SetCellValue(r, c + 3, pos.CustomerDisplay);
                        sheet.SetCellValue(r, c + 4, pos.BarcodeScanner);
                        sheet.SetCellValue(r, c + 5, pos.UPS);
                        sheet.SetCellValue(r, c + 6, pos.SerialPortsUsed);

                        c += 7;
                    }
                    else
                    {
                        sheet.SetCellValue(r, c + 2, pos.CustomerDisplay);
                        sheet.SetCellValue(r, c + 3, pos.BarcodeScanner);
                        sheet.SetCellValue(r, c + 4, pos.UPS);
                        sheet.SetCellValue(r, c + 5, pos.SerialPortsUsed);

                        c += 6;
                    }
                }

                var comms = value.Dispensers
                    .Select(x => x.Protocol).Distinct();

                c = 22;
                foreach (var proto in comms)
                {
                    sheet.SetCellValue(r, c, proto);
                    c++;
                }

                r++;
            }

            r++;

            Directory.CreateDirectory(outputPath);
            sheet.SaveAs($"{outputPath}/FuelPOS Survey.xlsx");
        }
    }
}
