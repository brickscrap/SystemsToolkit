using System.Xml.Serialization;

namespace Pse.TerminalsToEmis.Models
{
    [XmlType(TypeName = "language")]
    public class Language
    {
        [XmlAttribute(AttributeName = "lcid")]
        public string Lcid { get; set; }
    }
}