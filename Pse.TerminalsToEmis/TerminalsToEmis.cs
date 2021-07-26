using Pse.TerminalsToEmis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pse.TerminalsToEmis
{
    public static class TerminalsToEmis
    {
        public static void Run(string terminalsPath, string outputPath)
        {
            Settings settings = new();

            settings.Languages = new()
            {
                new Language { Lcid = "1033" }
            };

            settings.Stations = new();

            settings.Stations = File.ReadAllLines(terminalsPath)
                .Skip(2)
                .Select(v => Station.FromCsv(v))
                .Where(station => station is not null)
                .ToList();

            settings.DownloadServer.Url = "";
            settings.DownloadServer.Used = false;

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            outputPath = $"{outputPath}\\FmsSettings.xml";
            TextWriter writer = new StreamWriter(outputPath);

            serializer.Serialize(writer, settings);
        }
    }
}
