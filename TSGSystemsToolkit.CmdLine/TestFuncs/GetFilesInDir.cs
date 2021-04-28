using System;

namespace TSGSystemsToolkit.CmdLine.TestFuncs
{
    public class GetFilesInDir
    {
        public void Run()
        {
            string[] files = System.IO.Directory.GetFiles(@"C:\Users\omgit\Documents\For Nico", "*.jpg");

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}
