using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace POSFileParser
{
    public static class Helpers
    {
        private const string _dateTimeFormat = "yyyyMMddHHmmss";

        public static DateTime ParseFuelPOSDate(this string value)
        {
            return DateTime.ParseExact(value, _dateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}
