using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SendFileOptions : OptionsBase
    {
        public string FilePath { get; set; }
        public string Output { get; set; }
        public string Cluster { get; set; }
        public string Target { get; set; }
        public string List { get; set; }
        public string Site { get; set; }

        public SendFileOptions(string filepath, string output, string? cluster, string target, string? list, string? site)
        {
            FilePath = filepath;
            Output = output;
            Cluster = cluster;
            Target = target;
            List = list;
            Site = site;
        }
    }
}
