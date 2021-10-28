﻿using System;
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
            var newLine = line.Trim();
            var output = regex.Replace(newLine, ";").Trim();

            output = output.RemoveConsecutiveDuplicateChars(';');

            return output;
        }

        internal static string RemoveConsecutiveDuplicateChars(this string input, char character)
        {
            if (input.Length <= 1)
                return input;

            if (input[0] == input[1] && input[1] == character)
                return input.Substring(1)
                            .RemoveConsecutiveDuplicateChars(character);
            else
                return input[0] + input.Substring(1)
                                            .RemoveConsecutiveDuplicateChars(character);
        }
    }
}
