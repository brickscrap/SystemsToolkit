using System.Collections.Generic;
using System.Xml.Linq;
using ToolkitLibrary.Models;

namespace ToolkitLibrary
{
    public interface IStatdevParser
    {
        IEnumerable<XElement> MyProperty { get; set; }

        StatdevModel Parse(XDocument xmlDoc);
    }
}