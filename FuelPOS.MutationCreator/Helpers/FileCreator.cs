using System.Collections.Generic;
using System.IO;

namespace FuelPOS.MutationCreator.Helpers
{
    public static class FileCreator
    {
        public static void Create(this List<string> input, string fileName, string filePath)
        {
            using TextWriter tw = new StreamWriter($"{filePath}\\{fileName}");
            foreach (var s in input)
            {
                tw.WriteLine(s);
            }
        }
    }
}
