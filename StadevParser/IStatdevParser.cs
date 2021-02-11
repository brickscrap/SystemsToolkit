using StadevParser.Models;
using System.Xml.Linq;

namespace StadevParser
{
    public interface IStatdevParser
    {
        string GetTouchScreenTest();
        StatdevModel Parse(string xmlDoc);
        StatdevModel Parse(XDocument xmlDoc);
    }
}