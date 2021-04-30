using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser.Helpers
{
    internal static class StationHelpers
    {
        internal static string GetStationNumber(this IEnumerable<XElement> statDev)
        {
            var output = statDev.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "1")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }

        internal static string GetStationName(this IEnumerable<XElement> statDev)
        {
            var output = statDev.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "2")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }

        internal static string GetCompany(this IEnumerable<XElement> statDev)
        {
            var output = statDev.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "6")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static int GetNumberOfTills(this IEnumerable<XElement> statDev)
        {
            var number = statDev.Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "10")
                .FirstOrDefault();

            if (number != null)
            {
                var output = int.Parse(number.Value);

                return output;
            }

            return 0;
        }
    }
}
