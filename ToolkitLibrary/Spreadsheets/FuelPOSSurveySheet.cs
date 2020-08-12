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
                worksheet.Cells["J2"].Value = "Hardware";
                worksheet.Cells["K2"].Value = "BD";
                worksheet.Cells["L2"].Value = "CDU";
                worksheet.Cells["M2"].Value = "Scanner";
                worksheet.Cells["N2"].Value = "UPS";
                worksheet.Cells["O2"].Value = "Ser Ports Used";
                worksheet.Cells["P2"].Value = "Hardware";
                worksheet.Cells["Q2"].Value = "BD";
                worksheet.Cells["R2"].Value = "CDU";
                worksheet.Cells["S2"].Value = "Scanner";
                worksheet.Cells["T2"].Value = "UPS";
                worksheet.Cells["U2"].Value = "Ser Ports Used";
                worksheet.Cells["V2"].Value = "Protocol 1";
                worksheet.Cells["W2"].Value = "Protocol 2";
                worksheet.Cells["X2"].Value = "Protocol 3";

                var r = 3;

                foreach (var item in data)
                {
                    worksheet.Cells[r, 1].Value = item.StationInfo.StationNumber;
                    worksheet.Cells[r, 2].Value = item.StationInfo.StationName;

                    var col = 3;
                    foreach (var pos in item.POS)
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

                    var comms = item.Dispensers
                        .Select(x => x.Protocol).Distinct();

                    col = 22;
                    foreach (var protocol in comms)
                    {
                        worksheet.Cells[r, col].Value = protocol;
                        col++;
                    }

                    r++;
                }

                //for (int i = 0; i < data.Count; i++)
                //{
                //    worksheet.Cells[$"A{i + 3}"].Value = data[i].StationInfo.StationNumber;
                //    worksheet.Cells[$"B{i + 3}"].Value = data[i].StationInfo.StationName;
                //    worksheet.Cells[$"C{i + 3}"].Value = data[i].POS[0].HardwareType;
                //    worksheet.Cells[$"D{i + 3}"].Value = data[i].POS[0].OperatingSystem;
                //    worksheet.Cells[$"E{i + 3}"].Value = data[i].POS[0].SoftwareVersion;
                //    worksheet.Cells[$"F{i + 3}"].Value = data[i].POS[0].CustomerDisplay;
                //    worksheet.Cells[$"G{i + 3}"].Value = data[i].POS[0].BarcodeScanner;
                //    worksheet.Cells[$"H{i + 3}"].Value = data[i].POS[0].UPS;
                //    worksheet.Cells[$"I{i + 3}"].Value = data[i].POS[0].SerialPortsUsed;
                //    if (data[i].POS[1] != null)
                //    {
                //        worksheet.Cells[$"J{i + 3}"].Value = data[i].POS[1].HardwareType;
                //        worksheet.Cells[$"K{i + 3}"].Value = data[i].POS[1].OperatingSystem;
                //        worksheet.Cells[$"L{i + 3}"].Value = data[i].POS[1].CustomerDisplay;
                //        worksheet.Cells[$"M{i + 3}"].Value = data[i].POS[1].BarcodeScanner;
                //        worksheet.Cells[$"N{i + 3}"].Value = data[i].POS[1].UPS;
                //        worksheet.Cells[$"O{i + 3}"].Value = data[i].POS[1].SerialPortsUsed;
                //    }
                //    if (data[i].POS[2] != null)
                //    {
                //        worksheet.Cells[$"P{i + 3}"].Value = data[i].POS[2].HardwareType;
                //        worksheet.Cells[$"Q{i + 3}"].Value = data[i].POS[2].OperatingSystem;
                //        worksheet.Cells[$"R{i + 3}"].Value = data[i].POS[2].CustomerDisplay;
                //        worksheet.Cells[$"S{i + 3}"].Value = data[i].POS[2].BarcodeScanner;
                //        worksheet.Cells[$"T{i + 3}"].Value = data[i].POS[2].UPS;
                //        worksheet.Cells[$"U{i + 3}"].Value = data[i].POS[2].SerialPortsUsed;
                //    } 
                //}

                Utils.OutputDir = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}SampleApp");

                var xlFile = Utils.GetFileInfo("fuelpos_surveys.xlsx");
                // save our new workbook in the output directory and we are done!
                package.SaveAs(xlFile);
            }
        }
    } 
}
