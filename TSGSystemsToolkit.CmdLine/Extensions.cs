using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace TSGSystemsToolkit.CmdLine
{
    internal static class Extensions
    {
        internal static Version GetAvailableVersion(string folderPath)
        {
            Version ver = new();
            foreach (var file in Directory.EnumerateFiles(folderPath).Where(x => Path.GetFileNameWithoutExtension(x).StartsWith("systk_installer_")))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                var versionInfo = fileName.Split('_')[2];
                var splitVers = versionInfo.Split('.');
                Version availVersion = new(splitVers[0], splitVers[1], splitVers[2]);

                if (availVersion > ver)
                {
                    ver = availVersion;
                    ver.InstallerPath = file;
                }
            }

            return ver;
        }

        internal static bool IsUpdateAvailable(string installerLocation)
        {
            var fileVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version.Split('.');
            Version currentVersion = new(fileVersion[0], fileVersion[1], fileVersion[2]);

            try
            {
                Version available = GetAvailableVersion(installerLocation);
                return available > currentVersion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking for updates: {ex.Message}");
            }

            return false;
        }

        internal static void UpdateAccessToken(string accessToken)
        {
            var asPath = GetAppsettingsPath();
            var text = File.ReadAllText(asPath);

            var json = JsonSerializer.Deserialize<ExpandoObject>(text) as IDictionary<string, object>;

            json["AccessToken"] = accessToken;

            var newJson = JsonSerializer.Serialize<dynamic>(json,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            File.WriteAllText(asPath, newJson);
        }

        internal static string GetAppsettingsPath()
        {
            var absolutePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);

            return @$"{absolutePath}\appsettings.json";
        }
    }
}
