namespace TSGSystemsToolkit.CmdLine.Models
{
    internal class FtpTransferModel
    {
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
    }
}
