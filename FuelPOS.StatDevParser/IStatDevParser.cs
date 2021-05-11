using FuelPOS.StatDevParser.Models;
using System.Xml.Linq;

namespace FuelPOS.StatDevParser
{
    public interface IStatDevParser
    {
        StatdevModel Parse(XDocument xmlDoc);
    }
}