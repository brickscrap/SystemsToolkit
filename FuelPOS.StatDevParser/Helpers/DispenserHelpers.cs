using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser.Helpers
{
    internal static class DispenserHelpers
    {
        internal static string GetPumpProtocol(this IEnumerable<XElement> statDev, int pcNumber, int pumpNumber)
        {
            try
            {
                var output = statDev.SelectDispensers(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "9")
                .Where(value => (int)value.Attribute("Number") == pumpNumber)
                .Elements("Property")
                .Where(value => (int)value.Attribute("Type") == 60)
                .LastOrDefault();

                if (output is null)
                {
                    return null;
                }

                return output.Value;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
