using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WinSCP;

namespace FuelPOSFTPLib
{
    public class FtpConnection : IDisposable
    {
        private readonly SessionOptions _sessionOptions;
        private TransferOptions _transferOptions;
        private readonly ILogger<FtpConnection> _logger;

        internal Session Session { get; set; } = new Session();

        internal Protocol sessionProtocol
        {
            get { return _sessionOptions.Protocol; }
            set { _sessionOptions.Protocol = value; }
        }
        public string HostName
        {
            get { return _sessionOptions.HostName; }
            set { _sessionOptions.HostName = value; }
        }
        public string UserName
        {
            get { return _sessionOptions.UserName; }
            set { _sessionOptions.UserName = value; }
        }
        public string Password
        {
            private get { return _sessionOptions.Password; }
            set { _sessionOptions.Password = value; }
        }
        public int PortNumber
        {
            get { return _sessionOptions.PortNumber; }
            set { _sessionOptions.PortNumber = value; }
        }

        #region Constructors
        public FtpConnection(ILogger<FtpConnection> logger = null) : this("", "", "", 0, logger) 
        { }

        public FtpConnection(string hostName, string userName, string password, 
            ILogger<FtpConnection> logger = null) :
            this(hostName, userName, password, 0, logger)
        { }

        // TODO: Validation
        public FtpConnection(string hostName,
            string userName, string password, int portNumber,
            ILogger<FtpConnection> logger = null)
        {
            _logger = logger;
            _sessionOptions = new SessionOptions();
            sessionProtocol = Protocol.Ftp;
            HostName = hostName;
            UserName = userName;
            Password = password;
            PortNumber = portNumber;
            _transferOptions = new TransferOptions()
            {
                TransferMode = TransferMode.Binary,
            };
        }
        #endregion

        public void OpenSession()
        {
            try
            {
                Session.Open(_sessionOptions);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.InnerException}: {ex.Message}");
            };

            _logger?.LogInformation($"Connected to {HostName}");
        }

        public void CloseSession()
        {
            Session.Close();
        }

        /// <summary>
        /// Download files from a directory using specified path and filemask
        /// </summary>
        /// <param name="session">A WinSCP session object</param>
        /// <param name="remoteFilePath">The path to the remote directory, ending without a \</param>
        /// <param name="fileMask">The filemask, either a specific file name or *.EXT, filename.*,
        /// or * to download all files and directories.</param>
        /// <param name="destinationPath">The local directory which files will be downloaded to</param>
        /// <param name="remove">If true, remove files from host after download.</param>
        /// <returns>A list file paths of files downloaded.</returns>
        public List<string> DownloadFiles(string remoteFilePath, string fileMask, string destinationPath, bool remove)
        {
            List<string> result = new List<string>();
            try
            {
                TransferOperationResult transferResult;
                transferResult = Session.GetFilesToDirectory(remoteFilePath, destinationPath, fileMask, remove, this._transferOptions);

                transferResult.Check();

                foreach (TransferEventArgs item in transferResult.Transfers)
                {
                    _logger?.LogInformation($"Transfer of {item.FileName} success");
                }
                foreach (TransferEventArgs item in transferResult.Failures)
                {
                    _logger?.LogInformation($"Transfer of {item.FileName} failed, {item.Error}");
                }


                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    result.Add(transfer.Destination);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex.InnerException}: {ex.Message}");
            }
            
            // 

            

            return result;
        }

        // TODO: Complete/refactor
        public string DownloadMultipleFiles(string remoteDirectory, string fileType, string destinationPath, bool removeFile)
        {
            TransferOperationResult transferResult;
            var fileList = EnumerateRemoteFiles(remoteDirectory, "*.r00", EnumerationOptions.MatchDirectories);
            string result = "";


            foreach (RemoteFileInfo file in fileList)
            {
                if (!file.IsDirectory)
                {
                    transferResult = Session.GetFiles(file.FullName, destinationPath, removeFile, this._transferOptions);
                    transferResult.Check();
                    if (transferResult.IsSuccess)
                    {
                        _logger?.LogInformation($"Transfer of {transferResult.Transfers} success");
                    }
                    else
                    {
                        _logger?.LogError("Transfer failed");
                    }
                    
                }
            }

            return result;
        }

        public RemoteFileInfoCollection ListDirectory(string directoryLocation)
        {
            RemoteDirectoryInfo directory =
                Session.ListDirectory(directoryLocation);

            return directory.Files;
        }

        public IEnumerable<RemoteFileInfo> EnumerateRemoteFiles(string path, string mask, EnumerationOptions options)
        {
            IEnumerable<RemoteFileInfo> filesList = Session.EnumerateRemoteFiles(path, mask, options);
            return filesList;
        }

        public void Dispose()
        {
            ((IDisposable)Session).Dispose();
        }
    }
}
