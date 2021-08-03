using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
