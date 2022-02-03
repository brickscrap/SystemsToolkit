using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Models
{
    internal class FtpTransferModel
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
    }
}
