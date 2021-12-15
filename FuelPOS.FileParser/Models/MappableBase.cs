using SharpConfig;
using System;
using System.Collections.Generic;

namespace POSFileParser.Models
{
    public abstract class MappableBase<T>
    {
        private protected abstract IDictionary<string, Func<T, string, T>> Mappings { get; }
        internal virtual void MapValue(Setting setting, T model)
        {
            Func<T, string, T> function;

            if (Mappings.TryGetValue(setting.Name.StripKey(), out function))
            {
                function(model, setting.StringValue);
            }
        }
    }
}
