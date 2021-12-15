using System.Xml.Serialization;

namespace Pse.TerminalsToEmis.Models
{
    [XmlType(TypeName = "user")]
    public class User
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "pass")]
        public string Pass { get; set; }
    }
}
