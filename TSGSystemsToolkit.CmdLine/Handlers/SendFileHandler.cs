using FluentFTP;
using StrawberryShake;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Models;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

internal class SendFileHandler : ICommandHandler
{
    private SysTkApiClient _apiClient;
    private ILogger<SendFileHandler> _logger;
    private SendFileOptions _options;
    private CancellationToken _ct;

    public SendFileHandler(SendFileOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        // Validate Options - move to do this inside binders
        if (!ValidateOptions(_options))
            return 1;


        List<FtpTransferModel> transfers = await BuildTransfers(context);

        if (transfers is null || transfers.Count == 0)
            return 1;

        return RunTransfers(transfers);
    }

    private async Task<List<FtpTransferModel>> BuildTransfers(InvocationContext context)
    {
        List<FtpTransferModel> transfers = new();
        try
        {
            if (_options.Cluster is not null)
            {
                var result = await HandleClusterOption(context);

                if (result.isSuccess)
                    transfers.AddRange(result.transfers);
                else
                    return null;
            }

            // Get sites from list file
            if (_options.List is not null)
            {
                var result = await HandleListOption(context);

                if (result.isSuccess)
                    transfers.AddRange(result.transfers);
                else
                    return null;
            }

            // Get individual site
            if (_options.Site is not null)
            {
                var result = await HandleSiteOption(context);

                if (result.isSuccess)
                    transfers.AddRange(result.transfers);
                else
                    return null;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogCritical("Error connecting to data service: {Message}", ex.Message);
            _logger.LogDebug("Inner exception: {Inner}", ex.InnerException);
            _logger.LogDebug("Stack trace: {Trace}", ex.StackTrace);
            return null;
        }

        return transfers;
    }

    private async Task<(bool isSuccess, List<FtpTransferModel> transfers, IOperationResult failureResult)> HandleClusterOption(InvocationContext context)
    {
        Cluster cluster = Enum.Parse<Cluster>(_options.Cluster, true);
        List<FtpTransferModel> transfers = new();
        var result = await _apiClient.GetStationsByCluster.ExecuteAsync(cluster, _ct);

        result.EnsureNoErrors();

        foreach (var station in result.Data.Station)
        {
            var credentials = station.FtpCredentials
                .FirstOrDefault(x => x.Username.ToUpper() == "SUPERVISOR");

            if (credentials is null)
            {
                _logger.LogInformation("No supervisor credentials found for station {Id}: {Name}, skipping...", station.Id, station.Name);
                continue;
            }

            FtpTransferModel transfer = new()
            {
                SiteId = station.Id,
                Name = station.Name,
                IP = station.Ip,
                Port = 0,
                Username = credentials.Username,
                Password = credentials.Password,
                LocalPath = _options.FilePath,
                RemotePath = _options.Target
            };

            transfers.Add(transfer);
        }

        return (true, transfers, null);
    }

    private async Task<(bool isSuccess, List<FtpTransferModel> transfers, IOperationResult result)> HandleListOption(InvocationContext context)
    {
        List<FtpTransferModel> transfers = new();

        var listFile = File.ReadAllLines(_options.List).ToList();

        foreach (var id in listFile)
        {
            var result = await _apiClient.GetCredentialsByStationId.ExecuteAsync(id, _ct);

            result.EnsureNoErrors();

            var station = result.Data.Station.FirstOrDefault();
            if (station is null)
            {
                _logger.LogInformation("Station {Id} not found...", id);
                continue;
            }

            var credentials = station.FtpCredentials.FirstOrDefault(x => x.Username == "SUPERVISOR");

            // TODO: Handle supervisor credentials not existing, offer option to add
            if (credentials is null)
            {
                _logger.LogInformation("No supervisor credentials found for station {Id}: {Name}, skipping...", id, station.Name);
                continue;
            }

            FtpTransferModel transfer = new()
            {
                SiteId = id,
                Name = station.Name,
                IP = station.Ip,
                Port = 0,
                Username = credentials.Username,
                Password = credentials.Password,
                LocalPath = _options.FilePath,
                RemotePath = _options.Target
            };

            transfers.Add(transfer);
        }

        return (true, transfers, null);
    }

    public async Task<(bool isSuccess, List<FtpTransferModel> transfers, IOperationResult result)> HandleSiteOption(InvocationContext context)
    {
        List<FtpTransferModel> transfers = new();
        var result = await _apiClient.GetCredentialsByStationId.ExecuteAsync(_options.Site, _ct);

        result.EnsureNoErrors();

        var station = result.Data.Station.FirstOrDefault();
        var credentials = station.FtpCredentials.FirstOrDefault(x => x.Username == "SUPERVISOR");

        if (credentials is null)
        {
            _logger.LogInformation("No supervisor credentials found for station {Id}: {Name}, skipping...", _options.Site, station.Name);
        }
        else
        {
            FtpTransferModel transfer = new()
            {
                SiteId = _options.Site,
                Name = station.Name,
                IP = station.Ip,
                Port = 0,
                Username = station.FtpCredentials.Where(x => x.Username == "SUPERVISOR").FirstOrDefault().Username,
                Password = station.FtpCredentials.Where(x => x.Username == "SUPERVISOR").FirstOrDefault().Password,
                LocalPath = _options.FilePath,
                RemotePath = _options.Target
            };

            transfers.Add(transfer);
        }

        return (true, transfers, null);
    }

    private int RunTransfers(List<FtpTransferModel> transfers)
    {
        ThreadPool.GetMaxThreads(out int maxThreads, out int completionPortThreads);
        ThreadPool.SetMaxThreads(5, 5);

        List<Task> tasks = new();
        List<string> errors = new();

        AnsiConsole.Progress()
                    .AutoClear(false)
                    .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn(Spinner.Known.Aesthetic)
                })
                .Start(ctx =>
                {
                    foreach (var stat in transfers)
                    {
                        Task task = Task.Factory.StartNew((x) =>
                        {
                            try
                            {
                                ProgressTask progTask = ctx.AddTask(stat.Name, maxValue: 100, autoStart: true);

                                HandleTransfer(progTask, stat, errors);
                            }
                            catch (Exception ex)
                            {
                                errors.Add(ex.Message);
                                    // TODO: Better handle the various exceptions
                                    _logger.LogDebug("Inside Task: {Message}", ex.Message);
                                _logger.LogDebug("{Inner}", ex.InnerException);
                                _logger.LogDebug("{Trace}", ex.StackTrace);
                            }
                        }, TaskCreationOptions.AttachedToParent, cancellationToken: _ct);

                        tasks.Add(task);
                    }
                    try
                    {
                        Task.WaitAll(tasks.ToArray(), _ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error: {Error}", ex.Message);
                        _logger.LogDebug("Inner: {Inner}", ex.InnerException);
                        _logger.LogDebug("Trace: {Trace}", ex.StackTrace);
                    }
                });

        if (errors.Count > 0)
        {
            AnsiConsole.MarkupLine($"Error list:");
            foreach (var e in errors)
                AnsiConsole.MarkupLine(e);
        }

        AnsiConsole.MarkupLine($"File successfully uploaded to [green]{transfers.Count - errors.Count} sites[/], with [red]{errors.Count} failures.[/]");

        return 0;
    }

    private List<string> HandleTransfer(ProgressTask progTask, FtpTransferModel stat, List<string> errors)
    {
        using var ftpClient = new FtpClient(stat.IP, stat.Port, stat.Username, stat.Password);
        ftpClient.Connect();

        double increment = 0;
        double lastProg = 0;
        void prog(FtpProgress p)
        {
            if (increment == 0)
            {
                increment = p.Progress;
                lastProg = p.Progress;
            }
            else
            {
                increment = p.Progress - lastProg;
                lastProg = p.Progress;
            }

            progTask.Increment(increment);
        }

        var result = ftpClient.UploadFile(stat.LocalPath, stat.RemotePath, FtpRemoteExists.Overwrite, progress: prog);

        if (result is FtpStatus.Failed)
        {
            AnsiConsole.MarkupLine($"[red]Error uploading file to station:[/] [white]{stat.SiteId} - {stat.Name}[/]");
            errors.Add($"{stat.SiteId} - {stat.Name}");
        }

        return errors;
    }

    private bool ValidateOptions(SendFileOptions _options)
    {
        // TODO: Move this to Binder
        try
        {
            if (_options.FilePath.IsDirectory())
            {
                throw new FileNotFoundException($"{_options.FilePath} is a directory, please provide the path to a file.");
            }

            if (_options.List is not null && _options.List.IsDirectory())
            {
                throw new FileNotFoundException($"{_options.List} is a directory, please provide the path to a CSV or text file containing a list of station IDs.");
            }
        }
        catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
        {
            _logger.LogError("File or directory not found: {Message}", ex.Message);
            return false;
        }

        if (_options.Cluster is not null)
        {
            try
            {
                Cluster cluster = Enum.Parse<Cluster>(_options.Cluster, true);
            }
            catch (ArgumentException)
            {
                _logger.LogError("The provided cluster name \"{Cluster}\" is not a valid cluster.", _options.Cluster);
                var clusters = Enum.GetValues<Cluster>();
                Console.WriteLine("Possible clusters are:");
                foreach (var item in clusters)
                {
                    Console.WriteLine(item);
                }

                return false;
            }
        }

        return true;
    }

    private void GetDependencies(InvocationContext context)
    {
        _apiClient = context.BindingContext.GetService(typeof(SysTkApiClient)) as SysTkApiClient;
        _logger = context.BindingContext.GetService(typeof(ILogger<SendFileHandler>)) as ILogger<SendFileHandler>;
    }
}
