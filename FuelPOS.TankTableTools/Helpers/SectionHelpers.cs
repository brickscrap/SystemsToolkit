using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuelPOS.TankTableTools.Helpers
{
    internal static class SectionHelpers
    {
        internal static List<string> RemoveHeading(this List<string> section)
        {
            section.RemoveRange(0, 3);
            return section;
        }

        internal static string ToSemiColonSeparated(this string line)
        {
            var regex = new Regex("   +");
            var whitespaceRegex = new Regex(@"\s+");
            var newLine = line.Trim();
            newLine = regex.Replace(newLine, string.Empty);
            return regex.Replace(newLine, ";").Trim();
        }
    }
}
