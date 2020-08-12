using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpreadsheetUtils
{
    public class FuelPOSSurveySheet
    {
        public void CreateSurvey()
        {
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("FuelPOS Surveys");

                // Add headers
                worksheet.Cells["A1:B1"].Merge = true;
                worksheet.Cells["C1:I1"].Merge = true;
                worksheet.Cells["C1:I1"].Value = "POS 1";
                worksheet.Cells["J1:O1"].Merge = true;
                worksheet.Cells["J1:O1"].Value = "POS 2";
                worksheet.Cells["P1:U1"].Merge = true;
                worksheet.Cells["P1:U1"].Value = "POS 3";

                // Second row
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

                Utils.OutputDir = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}SampleApp");

                var xlFile = Utils.GetFileInfo("fuelpos_surveys.xlsx");
                // save our new workbook in the output directory and we are done!
                package.SaveAs(xlFile);
            }
        }
    }
}
