using SharpConfig;
using System.Linq;

namespace POSFileParser.Models
{
    public interface IFileSection
    {
        public IFileSection Create(IGrouping<string, Setting> groupedData);
    }
}
