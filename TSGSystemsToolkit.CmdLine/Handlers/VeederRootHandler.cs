using FuelPOS.TankTableTools;
using System.Collections.Generic;
using System.IO;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

public class VeederRootHandler : ICommandHandler
{
    private IVdrRootFileParser _parser;
    private ILogger<VeederRootHandler> _logger;
    private readonly VeederRootOptions _options;
    private readonly CancellationToken _ct;

    public VeederRootHandler(VeederRootOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    private void GetDependencies(InvocationContext context)
    {
        _parser = (IVdrRootFileParser)context.BindingContext.GetService(typeof(IVdrRootFileParser));
        _logger = (ILogger<VeederRootHandler>)context.BindingContext.GetService(typeof(ILogger<VeederRootHandler>));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        try
        {
            FileAttributes attr = File.GetAttributes(_options.FilePath);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                _logger.LogInformation("Parsing files in directory: {Directory}.", _options.FilePath);
                ParseFilesInDir();

                return 0;
            }
            else
            {
                ParseSingleFile();

                return 0;
            }
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            _logger.LogDebug("Inner Exception: {Inner}", ex.InnerException);
            _logger.LogDebug("Trace: {Trace}", ex.StackTrace);

            return -1;
        }
    }

    private void ParseSingleFile()
    {
        _parser = ParseFile();
        var outputDir = _options.OutputPath;

        if (string.IsNullOrWhiteSpace(_options.OutputPath))
        {
            outputDir = Path.GetDirectoryName(_options.FilePath);
        }

        HandleBoolOptions(outputDir);
    }

    private void HandleBoolOptions(string outputDir)
    {
        if (_options.CreateFuelPosFile)
        {
            _logger.LogInformation("Creating TMS_AOF.INP at {OutputDir}", outputDir);
            POSFileCreator.CreateTmsAofFile(_parser.TankTables, outputDir);
        }

        if (_options.CreateCsv)
        {
            _logger.LogInformation("Creating tank setup CSV at {OutputDir}", outputDir);
            try
            {
                POSFileCreator.CreateFuelPosSetupCsv(_parser as VdrRootFileParser, outputDir);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating file at {outputDir}\n" +
                    "{Message}", outputDir, ex.Message);
            }
        }
    }

    private void ParseFilesInDir()
    {
        List<string> filesToConvert = GetFilesInDirectory(_options.FilePath);

        foreach (var file in filesToConvert)
        {
            _parser.FilePath = file;
            _parser.Parse();

            var outputDir = CreateNewDirectoryName();

            HandleBoolOptions(outputDir);
        }
    }

    private string CreateNewDirectoryName()
    {
        string newDirectory;
        if (string.IsNullOrWhiteSpace(_parser.SiteName))
        {
            newDirectory = $"{_options.FilePath}\\{Path.GetFileNameWithoutExtension(_parser.FilePath)}";
        }
        else
        {
            newDirectory = $"{_options.FilePath}\\{_parser.SiteName}";
        }

        return newDirectory;
    }

    private IVdrRootFileParser ParseFile()
    {
        _parser.FilePath = _options.FilePath;

        _parser.Parse();

        return _parser;
    }

    private static List<string> GetFilesInDirectory(string directoryPath) =>
        Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(f => f.EndsWith(".cal") || f.EndsWith(".txt") || f.EndsWith(".cap"))
            .ToList();


}
