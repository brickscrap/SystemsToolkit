using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace POSFileParser
{
    public static class Helpers
    {
        private const string _dateTimeFormat = "yyyyMMddHHmmss";
        private readonly static char[] _matchOn = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        public static DateTime ParseFuelPOSDate(this string value)
        {
            return DateTime.ParseExact(value, _dateTimeFormat, CultureInfo.InvariantCulture);
        }

        public static string[] SplitKey(this string item)
        {
            var subString = item.IndexOfAny(_matchOn);

            if (subString != -1)
            {
                var output = item.Insert(subString, ",")
                .Split(',');
                return output;
            }
            else
            {
                string[] output = new string[] { item };
                return output;
            }
        }

        public static string StripKey(this string item)
        {
            var subString = item.IndexOfAny(_matchOn);

            if (subString != -1)
            {
                var output = item.Substring(0, subString);
                return output;
            }
            else
            {
                return item;
            }
        }

        public static bool StringToBool(this string item)
        {
            return item.Equals("YES");
        }
    }
}
