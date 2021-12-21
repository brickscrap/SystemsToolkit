using DocumentFormat.OpenXml.Spreadsheet;
using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SpreadsheetLight;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SysTk.Utils.Spreadsheets;

namespace SysTk.Utils
{
    public class SpreadsheetCreator
    {
        private readonly ILogger _logger;

        public SpreadsheetCreator(ILogger logger = null)
        {
            _logger = logger ?? NullLogger.Instance;
        }

        public void CreateFuelPosSurvey(List<StatdevModel> data, string outputPath)
        {
            FuelPosSurveySheet fpSheet = new(_logger);
            SLDocument doc = fpSheet.Create(data);

            SaveSpreadsheet(outputPath, "FuelPOS Survey", doc);
        }

        public void CreatePinPadSerialSurvey(List<StatdevModel> data, string outputPath)
        {
            PinPadSerialsSheet serialSheet = new(_logger);
            SLDocument doc = serialSheet.Create(data);

            SaveSpreadsheet(outputPath, "Serial Numbers", doc);
        }

        private void SaveSpreadsheet(string outputPath, string fileName, SLDocument doc)
        {
            string filePath = $@"{outputPath}\{fileName}.xlsx";
            _logger.LogInformation("Saving to {OutputPath}", filePath);
            doc.SaveAs(filePath);
        }
    }
}
