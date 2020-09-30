using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POSFileParser.Attributes;
using System.Reflection;
using System.Runtime.Serialization;
using System.Globalization;

namespace POSFileParser
{
    public class Mapper
    {
        public static void MapFile<T>(T item, string propName, string value, object obj) where T : new()
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var mapping = attributes.FirstOrDefault(a => a.GetType() == typeof(FieldNameAttribute));

                if (mapping != null)
                {
                    var mapsTo = mapping as FieldNameAttribute;
                    if (propName == mapsTo.Name)
                    {
                        // var newValue = Convert.ChangeType(value, property.PropertyType, null);
                        switch (property.PropertyType.Name)
                        {
                            case nameof(DateTime):
                                property.SetValue(obj, value.ParseFuelPOSDate());
                                break;
                            case nameof(Boolean):
                                property.SetValue(obj, value.Equals("YES"));
                                break;
                            case nameof(Int32):
                                property.SetValue(obj, int.Parse(value));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
