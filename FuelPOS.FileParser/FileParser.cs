using POSFileParser.Models;
using SharpConfig;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POSFileParser
{
    public static class FileParser
    {
        public static void LoadFile(string filePath)
        {
            var fileType = Path.GetExtension(filePath);
            var file = new Configuration();

            switch (fileType)
            {
                case ".TRX":
                    file = Configuration.LoadFromFile(filePath, Encoding.UTF8);
                    List<TRXModel> output = ParseTRXFile(file);
                    break;
                case ".PU":
                    file = Configuration.LoadFromFile(filePath, Encoding.Unicode);
                    ParsePUFile(file);
                    break;
            }
        }

        public static List<TRXModel> LoadTRXFile(string filePath)
        {
            var file = new Configuration();
            file = Configuration.LoadFromFile(filePath, Encoding.UTF8);

            List<TRXModel> output = ParseTRXFile(file);

            var final = output.Where(x => x.Type == TrxType.NormalTrx);

            return output;
        }

        public static List<TRXModel> ParseTRXFile(Configuration file)
        {
            List<IFileSection> output = new List<IFileSection>();

            foreach (Section section in file)
            {
                if (section.Name.Equals("TRX"))
                {
                    output = FileSectionFactory.CreateList(section);
                }
            }

            return output.ConvertAll(o => (TRXModel)o);
        }

        public static DayReportModel ParsePUFile(Configuration file)
        {
            DayReportModel dayReport = new DayReportModel();

            foreach (Section section in file)
            {
                switch (section.Name)
                {
                    case "STATUS":
                        dayReport.Status = (StatusModel)FileSectionFactory.CreateIndividual(section);
                        break;
                    default:
                        break;
                }
            }

            return dayReport;
        }

        public static List<T> Parse<T>(Section section) where T : ICanParse, new()
        {
            List<T> items = new List<T>();
            T newItem = new T();
            bool first = true;

            foreach (var item in section)
            {
                string[] headers = item.Name.SplitKey();

                if (headers.Length == 1)
                {
                    newItem.AddToItem(headers, item.StringValue);
                    continue;
                }
                else if (headers.Length > 1)
                {
                    if (first)
                    {
                        newItem.IDKey = headers[1];
                        first = false;
                    }

                    if (headers[1] == newItem.IDKey)
                    {
                        newItem.AddToItem(headers, item.StringValue);
                    }
                    else
                    {
                        items.Add(newItem);
                        newItem = new T { IDKey = headers[1] };

                        newItem.AddToItem(headers, item.StringValue);
                    }
                }

            }

            items.Add(newItem);

            return items;
        }
    }
}
