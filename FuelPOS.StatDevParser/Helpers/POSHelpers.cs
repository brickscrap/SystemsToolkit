using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser.Helpers
{
    internal static class POSHelpers
    {
        internal static string GetPCType(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "34")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static string GetOS(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "54")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static int GetPCNumber(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "68")
                .FirstOrDefault();

            if (output != null)
            {
                return int.Parse(output.Value);
            }

            return 0;
        }
        internal static string GetHardwareType(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "147")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static string GetSoftwareVersion(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "35")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static string GetIP(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "56")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
        internal static string GetReceiptPrinter(this IEnumerable<XElement> statDev, int pcNumber)
        {
            try
            {
                var output = statDev.SelectPC(pcNumber)
                        .Elements("Device")
                        .Where(item => (string)item.Attribute("Type") == "30")
                        .Elements("Property")
                        .Where(item => (string)item.Attribute("Type") == "34")
                        .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }
        internal static string GetCustDisplay(this IEnumerable<XElement> statDev, int pcNumber)
        {
            try
            {
                var output = statDev.SelectPC(pcNumber)
                        .Elements("Device")
                        .Where(item => (string)item.Attribute("Type") == "27")
                        .Elements("Property")
                        .Where(item => (string)item.Attribute("Type") == "34")
                        .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }
        internal static string GetBarcodeScanner(this IEnumerable<XElement> statDev, int pcNumber)
        {
            try
            {
                var output = statDev.SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "26")
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "34")
                .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }

        }
        internal static IEnumerable<XElement> GetSerialDevices(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "38")
                .Where(value => (int)value.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "39")
                .Where(value => (int)value.Attribute("Number") == pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "41");

            return output;
        }

        internal static string GetUPS(this IEnumerable<XElement> statDev, int pcNumber)
        {
            try
            {
                var output = statDev.SelectPC(pcNumber)
                .Elements("Device")
                .Where(value => (string)value.Attribute("Type") == "16")
                .Elements("Property")
                .Where(value => (string)value.Attribute("Type") == "28")
                .FirstOrDefault();

                if (output != null)
                {
                    return output.Value;
                }

                return "None/unknown";
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static string GetTouchscreenType(this IEnumerable<XElement> statDev, int pcNumber)
        {
            var output = statDev.SelectPC(pcNumber)
                .Elements("Device")
                .Where(item => (string)item.Attribute("Type") == "42")
                .Elements("Device")
                .Where(item => item.Value.Contains("TouchScreenType"))
                .Elements("Property")
                .Where(item => (string)item.Attribute("Type") == "89")
                .FirstOrDefault();

            if (output != null)
            {
                return output.Value;
            }

            return "None/unknown";
        }
    }
}
