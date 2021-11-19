using System.Threading;
using System.Threading.Tasks;

namespace SysTk.Utils
{
    public interface IFtpHandler
    {
        Task<bool> DownloadFileAsync(string host, string user, string password, string localPath, string remotePath, int port = 21, bool overwrite = false, bool deleteOnceDownloaded = false, CancellationToken ct = default);
        Task<bool> UploadFileAsync(string host, string user, string password, string localPath, string remotePath, int port = 21, bool overwrite = false, bool deleteOnceUploaded = false, CancellationToken ct = default);
    }
}