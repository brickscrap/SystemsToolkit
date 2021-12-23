using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using SysTk.Utils;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class SurveyHandler : IHandler<SurveyOptions>
    {
        private readonly ILogger<SurveyHandler> _logger;
        private readonly IStatDevParser _statDevParser;

        public SurveyHandler(IStatDevParser statDevParser, ILogger<SurveyHandler> logger = null)
        {
            _logger = logger ?? NullLogger<SurveyHandler>.Instance;
            _statDevParser = statDevParser;
        }

        public int RunHandlerAndReturnExitCode(SurveyOptions options)
        {
            int exitCode = 0;

            List<StatdevModel> statdevs = new();

            try
            {
                FileAttributes attr = File.GetAttributes(options.FilePath);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var xmlFiles = Directory.EnumerateFiles(options.FilePath, "*.xml");
                    _logger.LogInformation("Found {Count} XML files in {FilePath}", xmlFiles.Count(), options.FilePath);

                    foreach (var item in xmlFiles)
                    {
                        try
                        {
                            statdevs.Add(_statDevParser.Parse(item));
                        }
                        catch (XmlException ex)
                        {
                            _logger.LogError("Error in file {Item}", item);
                            _logger.LogError("{Message}", ex.Message);

                            continue;
                        }
                    }
                }
                else
                {
                    try
                    {
                        statdevs.Add(_statDevParser.Parse(options.FilePath));
                    }
                    catch (XmlException ex)
                    {
                        _logger.LogCritical("Error in file {Item}", options.FilePath);
                        _logger.LogCritical("{Message}", ex.Message);

                        return -1;
                    }

                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError("Error: {Message}", ex.Message);
                _logger.LogDebug("Inner Exception: {Inner}", ex.InnerException);
                _logger.LogDebug("Trace: {Trace}", ex.StackTrace);

                return -1;
            }

            if (options.FuelPosSurvey)
            {
                SpreadsheetCreator creator = new(_logger);
                creator.CreateSpreadsheet(SpreadsheetType.FuelPosSurvey, statdevs, options.OutputPath);
            }

            if (options.SerialNumberSurvey)
            {
                SpreadsheetCreator creator = new(_logger);
                creator.CreateSpreadsheet(SpreadsheetType.PinPadSerials, statdevs, options.OutputPath);
            }

            if (options.FuelPosSurvey || options.SerialNumberSurvey)
                Process.Start("explorer.exe", options.OutputPath);

            return exitCode;
        }
    }
}
