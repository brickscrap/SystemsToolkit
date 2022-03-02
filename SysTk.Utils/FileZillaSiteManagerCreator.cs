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
            XmlSerializer reader = new(typeof(FileZilla3));

            StreamReader file = new(siteManagerPath);
            FileZilla3 siteManager = (FileZilla3)reader.Deserialize(file);
            file.Dispose();

            var uniqueClusters = hosts.Select(x => x.Cluster)
                .Distinct()
                .ToList();

            uniqueClusters.Sort();

            Dictionary<string, List<Server>> folders = new();

            foreach (var cluster in uniqueClusters)
                folders.Add(cluster, new());

            foreach (var host in hosts)
            {
                var passBytes = Encoding.UTF8.GetBytes(host.Password);
                var passBase64 = Convert.ToBase64String(passBytes);
                folders[host.Cluster].Add(new Server()
                {
                    Host = host.Ip,
                    User = host.Username,
                    Pass = new()
                    {
                        Text = passBase64
                    },
                    Name = $"{host.Name} - {host.Id}"
                });
            }

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

            if (!siteManager.Servers.Folders.Exists(x => x.Name == "FuelPOS"))
                siteManager.Servers.Folders.Add(fuelPosFolder);
            else
            {
                var toRemove = siteManager.Servers.Folders.Where(x => x.Name == "FuelPOS").First();
                siteManager.Servers.Folders.Remove(toRemove);
                siteManager.Servers.Folders.Add(fuelPosFolder);
            }

            var x = new XmlSerializer(typeof(FileZilla3));
            var xml = "";

            using var sww = new StringWriter();
            using XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented };
            XmlSerializerNamespaces blankNs = new(new[] { new XmlQualifiedName("", "") });
            x.Serialize(writer, siteManager, blankNs);
            xml = sww.ToString();
            File.WriteAllText(siteManagerPath, xml);
        }
    }
}
