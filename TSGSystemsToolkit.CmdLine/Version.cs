using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine
{
    internal class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string InstallerPath { get; set; }

        public Version(string major, string minor, string patch)
        {
            Major = int.Parse(major);
            Minor = int.Parse(minor);
            Patch = int.Parse(patch);
        }

        public Version()
        {
            Major = 0;
            Minor = 0;
            Patch = 0;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public static bool operator >(Version a, Version b)
        {
            if (a.Major != b.Major)
                if (a.Major > b.Major)
                    return true;
                else
                    return false;

            if (a.Minor != b.Minor)
                if (a.Minor > b.Minor)
                    return true;
                else
                    return false;

            if (a.Patch != b.Patch)
                if (a.Patch > b.Patch)
                    return true;
                else
                    return false;

            return false;
        }

        public static bool operator <(Version a, Version b)
        {
            if (a.Major != b.Major)
                if (a.Major < b.Major)
                    return true;
                else
                    return false;

            if (a.Minor != b.Minor)
                if (a.Minor < b.Minor)
                    return true;
                else
                    return false;

            if (a.Patch != b.Patch)
                if (a.Patch < b.Patch)
                    return true;
                else
                    return false;

            return false;
        }
    }
}
