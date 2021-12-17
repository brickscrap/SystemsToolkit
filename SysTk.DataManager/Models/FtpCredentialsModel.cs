using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.DataManager.Models
{
    public class FtpCredentialsModel
    {
        public string Id { get; set; }
        public string Cluster { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastModified { get; set; }
    }
}
