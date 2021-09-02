using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SysTk.Utils;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Commands
{
    public class SurveyCommands
    {
        public static void RunSurveyCommands(SurveyOptions opts)
        {
            FileAttributes attr = File.GetAttributes(opts.InputPath);

            if (opts.CreateSurveySpreadsheet)
            {
                List<StatdevModel> statdevs = new();

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var xmlFiles = Directory.EnumerateFiles(opts.InputPath, "*.xml");

                    foreach (var item in xmlFiles)
                    {
                        var xdoc = new XDocument();
                        xdoc = XDocument.Load(item);

                        StatDevParser parser = new();

                        statdevs.Add(parser.Parse(xdoc));
                    }
                }
                else
                {

                }

                SpreadsheetCreator.CreateFuelPosSurvey(statdevs, opts.OutputPath);
            }
        }
    }
}
