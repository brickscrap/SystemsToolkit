using System.Collections.Generic;
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

        [XmlArray("Users")]
        public List<User> Users { get; set; }
        public DownloadServer DownloadServer { get; set; } = new();
    }
}
