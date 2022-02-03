using FluentFTP;
using FluentFTP.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SysTk.DataManager.Ftp
{
    public class FtpService : IFtpService
    {
        private readonly ILogger<FtpService> _logger;
        public FtpClient Client { get; set; }

        public FtpService(ILogger<FtpService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> UploadFileAsync(string host, string user, string password, string localPath,
                                                string remotePath, int port = 21, bool overwrite = false,
                                                bool deleteOnceUploaded = false,
                                                CancellationToken ct = default(CancellationToken))
        {
            using var ftp = CreateClient(host, port, user, password);
            ftp.OnLogEvent += Log;

            _logger.LogInformation("Connecting to {host}...", host);
            try
            {
                await ftp.ConnectAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}", ex.Message);
            }


            var overwriteExisting = overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip;

            _logger.LogInformation("Uploading \"{File}\" to \"{Target}\"...", localPath, remotePath);
            var result = await ftp.UploadFileAsync(localPath, remotePath, overwriteExisting);

            if (deleteOnceUploaded)
            {
                if (result.IsSuccess())
                {
                    try
                    {
                        File.Delete(localPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception when deleting local file {File}: {Exception}", localPath, ex.Message);
                        _logger.LogDebug("Inner exception: {Inner}", ex.InnerException);
                        _logger.LogDebug("Stack trace: {Trace}", ex.StackTrace);
                    }
                }
            }

            return result.IsSuccess();
        }

        public bool UploadFile(string host, string user, string password, string localPath,
                                                string remotePath, int port = 21, bool overwrite = false,
                                                bool deleteOnceUploaded = false, IProgress<double> progress = null)
        {
            using var ftp = CreateClient(host, port, user, password);
            ftp.OnLogEvent += Log;

            _logger.LogInformation("Connecting to {host}...", host);
            try
            {
               ftp.Connect();
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}", ex.Message);
            }

            Action<FtpProgress> progressAction = (x) => { };

            var overwriteExisting = overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip;

            if (progress is not null)
            {
                progressAction = delegate (FtpProgress p)
                {
                    if (p.Progress == 100)
                    {
                        progress.Report(100);
                    }
                    else
                    {
                        progress.Report(p.Progress / 100);
                    }
                };
            }

            _logger.LogInformation("Uploading \"{File}\" to \"{Target}\"...", localPath, remotePath);
            var result = ftp.UploadFile(localPath, remotePath, overwriteExisting, progress: progressAction);

            if (deleteOnceUploaded)
            {
                if (result.IsSuccess())
                {
                    try
                    {
                        File.Delete(localPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception when deleting local file {File}: {Exception}", localPath, ex.Message);
                        _logger.LogDebug("Inner exception: {Inner}", ex.InnerException);
                        _logger.LogDebug("Stack trace: {Trace}", ex.StackTrace);
                    }
                }
            }

            return result.IsSuccess();
        }

        public async Task<bool> DownloadFileAsync(string host, string user, string password, string localPath,
                                                  string remotePath, int port = 21, bool overwrite = false,
                                                  bool deleteOnceDownloaded = false,
                                                  CancellationToken ct = default(CancellationToken))
        {
            using var ftp = CreateClient(host, port, user, password);
            ftp.OnLogEvent += Log;
            await ftp.ConnectAsync(ct);

            var overwriteExisting = overwrite ? FtpLocalExists.Overwrite : FtpLocalExists.Skip;

            var result = await ftp.DownloadFileAsync(localPath, remotePath, overwriteExisting, token: ct);

            if (deleteOnceDownloaded)
            {
                if (result.IsSuccess())
                {
                    try
                    {
                        await ftp.DeleteFileAsync(remotePath, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception when deleting remote file {File}: {Exception}", remotePath, ex.Message);
                        _logger.LogDebug("Inner exception: {Inner}", ex.InnerException);
                        _logger.LogDebug("Stack trace: {Trace}", ex.StackTrace);
                    }
                }
            }

            return result.IsSuccess();
        }

        private static FtpClient CreateClient(string host, int port, string username, string password) =>
            new FtpClient(host, port, username, password);

        private void Log(FtpTraceLevel traceLevel, string content)
        {
            switch (traceLevel)
            {
                case FtpTraceLevel.Verbose:
                    _logger.LogDebug(content);
                    break;
                case FtpTraceLevel.Info:
                    _logger.LogDebug(content);
                    break;
                case FtpTraceLevel.Warn:
                    _logger.LogWarning(content);
                    break;
                case FtpTraceLevel.Error:
                    _logger.LogError(content);
                    break;
                default:
                    break;
            }
        }
    }
}
