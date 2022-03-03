using System.Xml.Serialization;

namespace FuelPOS.EmisFileCreator.Models
{
    public class DownloadServer
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; } = "";
        [XmlAttribute(AttributeName = "used")]
        public bool Used { get; set; } = false;
    }
}