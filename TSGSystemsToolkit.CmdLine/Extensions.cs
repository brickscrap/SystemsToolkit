using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TSGSystemsToolkit.CmdLine
{
    internal static class Extensions
    {
        internal static Command WithHandler<T>(this Command command, string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;
            var method = typeof(T).GetMethod(name, flags);

            var handler = CommandHandler.Create(method!);
            command.Handler = handler;

            return command;
        }

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
    }
}
