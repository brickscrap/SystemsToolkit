using System.Xml.Serialization;

namespace Pse.TerminalsToEmis
{
    [XmlType(TypeName = "language")]
    public class Language
    {
        [XmlAttribute(AttributeName = "lcid")]
        public string Lcid { get; set; }
    }
}