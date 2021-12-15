using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser.Helpers
{
    internal static class SectionSelectHelpers
    {
        public static IEnumerable<XElement> GetDispenserXML(this IEnumerable<XElement> xmlDoc, int pcNumber)
        {
            var output = xmlDoc.SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(value => (string)value.Attribute("Type") == "8");

            return output;
        }

        public static XElement SelectPC(this IEnumerable<XElement> xmlDoc, int pcNumber)
        {
            var output = xmlDoc.Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "2" && value.Attribute("Number") != null)
                .Where(number => (int)number.Attribute("Number") == pcNumber)
                .FirstOrDefault();

            return output;
        }
        public static XElement SelectDispensers(this IEnumerable<XElement> xmlDoc, int pcNumber)
        {
            var output = xmlDoc.SelectPC(pcNumber)
                    .Elements("Device")
                    .Where(value => (string)value.Attribute("Type") == "8")
                    .Where(value => (int)value.Attribute("Number") == pcNumber)
                    .FirstOrDefault();

            return output;
        }
    }
}
