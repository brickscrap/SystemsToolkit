using Newtonsoft.Json;
using POSFileParser;
using System.IO;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    internal static class TrxParserTests
    {
        public static void Run()
        {
            var output = FileParser.LoadTRXFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\11841179.TRX");
            // Parser.LoadFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\61111042.PU");

            JsonSerializerSettings settings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(output, Formatting.Indented, settings);

            File.WriteAllText(@"C:\Users\omgit\source\repos\FuelPOSToolkit\POSFileParser\Tests\trx.json", json);
        }
    }
}
