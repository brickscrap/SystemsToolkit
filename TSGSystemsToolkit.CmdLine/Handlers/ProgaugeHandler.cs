using FuelPOS.TankTableTools;
using System.Collections.Generic;
using System.IO;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

public class ProgaugeHandler : ICommandHandler
{
    // TODO: Add logging/output
    private ILogger<ProgaugeHandler> _logger;
    private IProgaugeFileParser _parser;
    private readonly ProgaugeOptions _options;
    private readonly CancellationToken _ct;

    public ProgaugeHandler(ProgaugeOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    private void GetDependencies(InvocationContext context)
    {
        _logger = context.BindingContext.GetService(typeof(ILogger<ProgaugeHandler>)) as ILogger<ProgaugeHandler>;
        _parser = context.BindingContext.GetService(typeof(IProgaugeFileParser)) as IProgaugeFileParser;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        ParseBasicFileInDir();

        return 1;
    }

    private void ParseBasicFileInDir()
    {
        _parser.FolderPath = _options.FilePath;

        _parser.LoadFilesAndParse();

        if (_parser.TankTables is not null)
        {
            if (_options.CreateFuelPosFile)
            {
                POSFileCreator.CreateTmsAofFile(_parser.TankTables, _options.FilePath);
            }
        }
    }

    private static List<string> GetFilesInDirectory(string directoryPath)
    {
        return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(f => f.EndsWith("*.cal") || f.EndsWith("*.txt") || f.EndsWith("*.cap"))
            .ToList();
    }
}
