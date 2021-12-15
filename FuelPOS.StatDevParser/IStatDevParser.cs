using FuelPOS.StatDevParser.Models;

namespace FuelPOS.StatDevParser
{
    public interface IStatDevParser
    {
        StatdevModel Parse(string xmlDocPath);
    }
}