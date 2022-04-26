using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SysTk.Utils;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

public class SurveyHandler : ICommandHandler
{
    private ILogger<SurveyHandler> _logger;
    private IStatDevParser _statDevParser;
    private readonly SurveyOptions _options;
    private readonly CancellationToken _ct;

    public SurveyHandler(SurveyOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        int exitCode = 0;

        List<StatdevModel> statdevs = new();

        try
        {
            FileAttributes attr = File.GetAttributes(_options.FilePath);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var xmlFiles = Directory.EnumerateFiles(_options.FilePath, "*.xml");
                _logger.LogInformation("Found {Count} XML files in {FilePath}", xmlFiles.Count(), _options.FilePath);

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
                    statdevs.Add(_statDevParser.Parse(_options.FilePath));
                }
                catch (XmlException ex)
                {
                    _logger.LogCritical("Error in file {Item}", _options.FilePath);
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

        if (_options.FuelPosSurvey)
        {
            SpreadsheetCreator creator = new(_logger);
            creator.CreateSpreadsheet(SpreadsheetType.FuelPosSurvey, statdevs, _options.OutputPath);
        }

        if (_options.SerialNumberSurvey)
        {
            SpreadsheetCreator creator = new(_logger);
            creator.CreateSpreadsheet(SpreadsheetType.PinPadSerials, statdevs, _options.OutputPath);
        }

        if (_options.FuelPosSurvey || _options.SerialNumberSurvey)
        {
            Process.Start("explorer.exe", _options.OutputPath);
        }

        return exitCode;
    }

    private void GetDependencies(InvocationContext context)
    {
        _logger = context.BindingContext.GetService(typeof(ILogger<SurveyHandler>)) as ILogger<SurveyHandler>;
        _statDevParser = context.BindingContext.GetService(typeof(IStatDevParser)) as IStatDevParser;
    }
}
