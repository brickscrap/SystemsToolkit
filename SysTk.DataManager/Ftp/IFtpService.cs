using FluentFTP;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SysTk.DataManager.Ftp
{
    public interface IFtpService
    {
        FtpClient Client { get; set; }

        Task<bool> DownloadFileAsync(string host, string user, string password, string localPath, string remotePath, int port = 21, bool overwrite = false, bool deleteOnceDownloaded = false, CancellationToken ct = default);
        bool UploadFile(string host, string user, string password, string localPath, string remotePath, int port = 21, bool overwrite = false, bool deleteOnceUploaded = false, IProgress<double> progress = null);
        Task<bool> UploadFileAsync(string host, string user, string password, string localPath, string remotePath, int port = 21, bool overwrite = false, bool deleteOnceUploaded = false, CancellationToken ct = default);
    }
}