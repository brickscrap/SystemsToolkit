using Pse.TerminalsToEmis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Pse.TerminalsToEmis
{
    public static class TerminalsToEmis
    {
        public static void Run(string terminalsPath, string outputPath)
        {
            Run(terminalsPath, outputPath, null);
        }

        public static void Run(string terminalsPath, string outputPath, string userName)
        {
            Settings settings = GetExistingXml(outputPath);

            settings.Languages = new()
            {
                new Language { Lcid = "1033" }
            };

            List<Station> stationsFromTerminals = new();

            stationsFromTerminals = File.ReadAllLines(terminalsPath)
                .Skip(2)
                .Select(v => Station.FromCsv(v))
                .Where(station => station is not null)
                .ToList();

            foreach (var existing in settings.Stations)
            {
                if (existing.Name.StartsWith('!'))
                    stationsFromTerminals.Add(existing);
            }

            settings.Stations = stationsFromTerminals;
            settings.DownloadServer.Url = "";
            settings.DownloadServer.Used = false;
            settings.Users = new();

            if (userName is not null)
                settings.Users.Add(new User { Name = userName, Pass = "" });

            XmlSerializer serializer = new(typeof(Settings));

            outputPath = $"{outputPath}\\FmsSettings.xml";
            using TextWriter writer = new StreamWriter(outputPath);

            serializer.Serialize(writer, settings);

            writer.Close();
        }

        private static Settings GetExistingXml(string xmlPath)
        {
            XmlSerializer reader = new(typeof(Settings));

            StreamReader file = new($"{xmlPath}\\FmsSettings.xml");
            Settings settings = (Settings)reader.Deserialize(file);
            file.Dispose();

            return settings;
        }
    }
}
