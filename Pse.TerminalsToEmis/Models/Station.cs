using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pse.TerminalsToEmis.Models
{
    [XmlType(TypeName = "station")]
    public class Station
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ipaddress")]
        public string IpAddress { get; set; }

        [XmlAttribute(AttributeName = "used")]
        public bool Used { get; set; } = false;

        public static Station FromCsv(string csvLine)
        {

            string[] values = csvLine.Split(';');

            if (values[17].Contains("TSG IE") || !values[13].Contains('.'))
            {
                return null;
            }

            char[] trimChars = { '\\', '=', '"' };
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim(trimChars).Trim();
            }

            var description = values[7].Trim('-').Split('-');
            if (description.Length < 3)
            {
                return null;
            }

            var name = $"{description[2].Trim()} - {description[0].Trim()} - {values[2].Trim()}";

            Station output = new()
            {
                Name = name,
                IpAddress = values[13],
                Used = false
            };

            return output;
        }
    }
}
