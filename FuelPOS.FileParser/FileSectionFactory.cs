using POSFileParser.Models;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSFileParser
{
    public static class FileSectionFactory
    {
        public static List<IFileSection> CreateList(Section fileSection)
        {
            List<IFileSection> output = new List<IFileSection>();

            switch (fileSection.Name)
            {
                case "TRX":
                    var separatedObjects = fileSection.GroupBy(r => Helpers.SplitKey(r.Name)[1]);

                    foreach (var entry in separatedObjects)
                    {
                        Dictionary<string, string> dict = entry.ToDictionary(
                            x => Helpers.SplitKey(x.Name).Length > 2 ? $"{Helpers.SplitKey(x.Name)[0]}{Helpers.SplitKey(x.Name)[2]}" : $"{Helpers.SplitKey(x.Name)[0]}",
                            y => y.StringValue);
                        IFileSection trx = new TRXModel();
                        trx = trx.Create(entry);
                        output.Add(trx);
                    }
                    return output;
                case "STATUS":
                    Dictionary<string, string> statusDict = new Dictionary<string, string>();
                    foreach (var entry in fileSection)
                    {
                        statusDict.Add(entry.Name, entry.StringValue);
                    }
                    IFileSection status = new StatusModel();
                    //output.Add(status.Create(statusDict));
                    return output;
                default:
                    break;
            }

            return null;
        }

        public static IFileSection CreateIndividual(Section fileSection)
        {
            IFileSection output;

            switch (fileSection.Name)
            {
                case "STATUS":
                    Dictionary<string, string> statusDict = new Dictionary<string, string>();
                    foreach (var entry in fileSection)
                    {
                        statusDict.Add(entry.Name, entry.StringValue);
                    }
                    IFileSection status = new StatusModel();
                    // output = status.Create(statusDict);
                    //return output;
                    return null;
                default:
                    throw new Exception("Unable to create individual section");
            }
        }

        public enum FileSection
        {
            TRX = 1,
            STATUS = 2
        }
    }
}
