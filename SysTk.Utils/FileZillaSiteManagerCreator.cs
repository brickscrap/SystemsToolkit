using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SysTk.Utils.FileZilla;

namespace SysTk.Utils
{
    public static class FileZillaSiteManagerCreator
    {
        public static void Run(List<HostModel> hosts, string siteManagerPath)
        {
            FileZilla3 siteManager = GetExistingXml(siteManagerPath);

            var uniqueClusters = GetUniqueClusters(hosts);

            Dictionary<string, List<Server>> folders = CreateFoldersWithServers(hosts, uniqueClusters);

            var fuelPosFolder = CreateFuelPosFolder(folders);

            siteManager.CheckFuelPosFolderExists(fuelPosFolder);

            CreateXmlFile(siteManager, siteManagerPath);
        }

        private static FileZilla3 GetExistingXml(string filePath)
        {
            XmlSerializer reader = new(typeof(FileZilla3));

            StreamReader file = new(filePath);
            FileZilla3 siteManager = (FileZilla3)reader.Deserialize(file);
            file.Dispose();

            return siteManager;
        }

        private static List<string> GetUniqueClusters(List<HostModel> hosts)
        {
            var uniqueClusters = hosts.Select(x => x.Cluster)
                .Distinct()
                .ToList();

            uniqueClusters.Sort();

            return uniqueClusters;
        }

        private static string ConvertToBase64(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        private static Dictionary<string, List<Server>> CreateFoldersWithServers(List<HostModel> hosts, List<string> uniqueClusters)
        {
            Dictionary<string, List<Server>> folders = new();

            foreach (var cluster in uniqueClusters)
                folders.Add(cluster, new());

            foreach (var host in hosts)
            {
                folders[host.Cluster].Add(new Server()
                {
                    Host = host.Ip,
                    User = host.Username,
                    Pass = new()
                    {
                        Text = ConvertToBase64(host.Password)
                    },
                    Name = $"{host.Name} - {host.Id}"
                });
            }

            return folders;
        }

        private static FileZilla3 CheckFuelPosFolderExists(this FileZilla3 siteManager, Folder fuelPosFolder)
        {
            if (!siteManager.Servers.Folders.Exists(x => x.Name == "FuelPOS"))
                siteManager.Servers.Folders.Add(fuelPosFolder);
            else
            {
                var toRemove = siteManager.Servers.Folders.Where(x => x.Name == "FuelPOS").First();
                siteManager.Servers.Folders.Remove(toRemove);
                siteManager.Servers.Folders.Add(fuelPosFolder);
            }

            return siteManager;
        }

        private static Folder CreateFuelPosFolder(Dictionary<string, List<Server>> folders)
        {
            var fuelPosFolder = new Folder("FuelPOS");

            foreach (var key in folders.Keys)
            {
                fuelPosFolder.Folders.Add(new()
                {
                    Name = key,
                    Expanded = "0",
                    Servers = folders[key]
                });
            }

            return fuelPosFolder;
        }

        private static void CreateXmlFile(FileZilla3 siteManager, string filePath)
        {
            var x = new XmlSerializer(typeof(FileZilla3));
            var xml = "";

            using var sww = new StringWriter();
            using XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented };
            XmlSerializerNamespaces blankNs = new(new[] { new XmlQualifiedName("", "") });
            x.Serialize(writer, siteManager, blankNs);
            xml = sww.ToString();
            File.WriteAllText(filePath, xml);
        }
    }
}
