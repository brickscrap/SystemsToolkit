using System.Xml.Serialization;

namespace FuelPOS.EmisFileCreator.Models
{
    [XmlType(TypeName = "language")]
    public class Language
    {
        [XmlAttribute(AttributeName = "lcid")]
        public string Lcid { get; set; }
    }
}