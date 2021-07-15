using System;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Serialization;

namespace Pse.TerminalsToEmis
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "";

            if (args.Length == 0)
            {
                Console.WriteLine("Path to Terminals_044.csv: ");
                filePath = Console.ReadLine();
            }

            Settings settings = new();

            settings.Languages = new()
            {
                new Language { Lcid = "1033" }
            };

            settings.Stations = new();

            settings.Stations = File.ReadAllLines(filePath)
                .Skip(2)
                .Select(v => Station.FromCsv(v))
                .Where(station => station is not null)
                .ToList();

            settings.DownloadServer.Url = "";
            settings.DownloadServer.Used = false;

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            var outputPath = $"{Path.GetDirectoryName(filePath)}\\FmsSettings.xml";
            TextWriter writer = new StreamWriter(outputPath);

            serializer.Serialize(writer, settings);

            Console.WriteLine($"Remote eMIS XML Generated:");
            Console.WriteLine($"Output: {outputPath}");
            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }
    }
}
