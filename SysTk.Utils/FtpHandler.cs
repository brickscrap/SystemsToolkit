using FluentFTP;
using FluentFTP.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SysTk.Utils
{
    public class FtpHandler : IDisposable, IFtpHandler
    {
        private readonly ILogger<FtpHandler> _logger;
        private FtpClient Client { get; set; }

        public FtpHandler(ILogger<FtpHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> UploadFileAsync(string host,
                                          string user,
                                          string password,
                                          string localPath,
                                          string remotePath,
                                          int port = 21,
                                          bool overwrite = false,
                                          bool deleteOnceUploaded = false,
                                          CancellationToken ct = default(CancellationToken))
        {
            using (var ftp = CreateClient(host, port, user, password))
            {
                ftp.OnLogEvent += Log;

                await ftp.ConnectAsync(ct);

                var overwriteExisting = overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip;

                var result = await ftp.UploadFileAsync(localPath, remotePath, overwriteExisting, token: ct);

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
        }

        public async Task<bool> DownloadFileAsync(string host,
                                                  string user,
                                                  string password,
                                                  string localPath,
                                                  string remotePath,
                                                  int port = 21,
                                                  bool overwrite = false,
                                                  bool deleteOnceDownloaded = false,
                                                  CancellationToken ct = default(CancellationToken))
        {
            using (var ftp = CreateClient(host, port, user, password))
            {
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
        }

        private static FtpClient CreateClient(string host, int port, string user, string password) =>
            new FtpClient(host, port, user, password);

        private void Log(FtpTraceLevel traceLevel, string content)
        {
            switch (traceLevel)
            {
                case FtpTraceLevel.Verbose:
                    _logger.LogDebug(content);
                    break;
                case FtpTraceLevel.Info:
                    _logger.LogInformation(content);
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

        public void Dispose() => Client?.Dispose();
    }
}
