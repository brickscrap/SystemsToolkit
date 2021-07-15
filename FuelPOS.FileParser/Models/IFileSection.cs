using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSFileParser.Models
{
    public interface IFileSection
    {
        public IFileSection Create(IGrouping<string, Setting> groupedData);
    }
}
