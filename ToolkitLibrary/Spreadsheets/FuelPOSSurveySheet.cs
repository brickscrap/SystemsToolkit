using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToolkitLibrary.Models;
using OfficeOpenXml.Style;

namespace ToolkitLibrary.Spreadsheets
{
    public class FuelPOSSurveySheet
    {
        // TODO: Fix this monstrosity
        public void CreateSurvey(List<StatdevModel> data)
        {
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("FuelPOS Surveys");

                var headerStyle = package.Workbook.Styles.CreateNamedStyle("Headers");
                headerStyle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerStyle.Style.Font.Bold = true;

                // Add headers
                worksheet.Cells["A1:B1"].Merge = true;
                using (var range = worksheet.Cells["C1:I1"])
                {
                    range.Merge = true;
                    range.Value = "POS 1";
                    range.StyleName = "Headers";
                }
                using (var range = worksheet.Cells["J1:O1"])
                {
                    range.Merge = true;
                    range.Value = "POS 2";
                    range.StyleName = "Headers";
                }
                using (var range = worksheet.Cells["P1:U1"])
                {
                    range.Merge = true;
                    range.Value = "POS 3";
                    range.StyleName = "Headers";
                }


                // Second row
                using (var range = worksheet.Cells["A2:X2"])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Font.Bold = true;
                }
                worksheet.Cells["A2"].Value = "Petrol Server";
                worksheet.Cells["B2"].Value = "Site Name";
                worksheet.Cells["C2"].Value = "Hardware";
                worksheet.Cells["D2"].Value = "BD";
                worksheet.Cells["E2"].Value = "SW Ver";
                worksheet.Cells["F2"].Value = "CDU";
                worksheet.Cells["G2"].Value = "Scanner";
                worksheet.Cells["H2"].Value = "UPS";
                worksheet.Cells["I2"].Value = "Ser Ports Used";
                worksheet.Cells["J2"].Value = "Hardware 2";
                worksheet.Cells["K2"].Value = "BD 2";
                worksheet.Cells["L2"].Value = "CDU 2";
                worksheet.Cells["M2"].Value = "Scanner 2";
                worksheet.Cells["N2"].Value = "UPS 2";
                worksheet.Cells["O2"].Value = "Ser Ports Used 2";
                worksheet.Cells["P2"].Value = "Hardware 3";
                worksheet.Cells["Q2"].Value = "BD 3";
                worksheet.Cells["R2"].Value = "CDU 3";
                worksheet.Cells["S2"].Value = "Scanner 3";
                worksheet.Cells["T2"].Value = "UPS 3";
                worksheet.Cells["U2"].Value = "Ser Ports Used 3";
                worksheet.Cells["V2"].Value = "Protocol 1";
                worksheet.Cells["W2"].Value = "Protocol 2";
                worksheet.Cells["X2"].Value = "Protocol 3";

                var r = 3;

                foreach (var value in data)
                {
                    worksheet.Cells[r, 1].Value = value.StationInfo.StationNumber;
                    worksheet.Cells[r, 2].Value = value.StationInfo.StationName;

                    var col = 3;
                    foreach (var pos in value.POS)
                    {
                        worksheet.Cells[r, col].Value = pos.HardwareType;
                        worksheet.Cells[r, col+1].Value = pos.OperatingSystem;
                        if (pos.Number == 1)
                        {
                            worksheet.Cells[r, col + 2].Value = pos.SoftwareVersion;
                            worksheet.Cells[r, col + 3].Value = pos.CustomerDisplay;
                            worksheet.Cells[r, col + 4].Value = pos.BarcodeScanner;
                            worksheet.Cells[r, col + 5].Value = pos.UPS;
                            worksheet.Cells[r, col + 6].Value = pos.SerialPortsUsed;

                            col = col + 7;
                        }
                        else
                        {
                            worksheet.Cells[r, col + 2].Value = pos.CustomerDisplay;
                            worksheet.Cells[r, col + 3].Value = pos.BarcodeScanner;
                            worksheet.Cells[r, col + 4].Value = pos.UPS;
                            worksheet.Cells[r, col + 5].Value = pos.SerialPortsUsed;

                            col = col + 6;
                        }
                    }

                    var comms = value.Dispensers
                        .Select(x => x.Protocol).Distinct();

                    col = 22;
                    foreach (var protocol in comms)
                    {
                        worksheet.Cells[r, col].Value = protocol;
                        col++;
                    }

                    r++;
                }

                r++;

                Utils.OutputDir = new DirectoryInfo($"C:/Output");

                var xlFile = Utils.GetFileInfo("fuelpos_surveys.xlsx");
                // save our new workbook in the output directory and we are done!
                package.SaveAs(xlFile);
            }
        }
    } 
}
