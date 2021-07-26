using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pse.TerminalsToEmis.Models
{
    [XmlRoot("Settings")]
    public class Settings
    {
        [XmlArray("languages")]
        public List<Language> Languages { get; set; }

        [XmlArray("stations")]
        public List<Station> Stations { get; set; }

        public string Users { get; set; }
        public DownloadServer DownloadServer { get; set; } = new();
    }
}
