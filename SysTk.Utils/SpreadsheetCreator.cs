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

        public void CreateSpreadsheet(SpreadsheetType type, List<StatdevModel> data, string outputPath)
        {
            switch (type)
            {
                case SpreadsheetType.FuelPosSurvey:
                    Create(new FuelPosSurveySheet(_logger), data, outputPath);
                    break;
                case SpreadsheetType.PinPadSerials:
                    Create(new PinPadSerialsSheet(_logger), data, outputPath);
                    break;
                default:
                    break;
            }
        }

        private void Create<T>(T spreadsheet, IEnumerable<StatdevModel> data, string outputPath) where T : ISpreadsheet<StatdevModel>
        {
            SLDocument doc = spreadsheet.Create(data);

            SaveSpreadsheet(outputPath, spreadsheet.Name, doc);
        }

        private void SaveSpreadsheet(string outputPath, string fileName, SLDocument doc)
        {
            string filePath = $@"{outputPath}\{fileName}.xlsx";
            _logger.LogInformation("Saving to {OutputPath}", filePath);
            doc.SaveAs(filePath);
        }
    }
}
