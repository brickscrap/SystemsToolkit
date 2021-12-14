using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using SysTk.Utils;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class SurveyHandler : ISurveyHandler
    {
        private readonly ILogger<SurveyHandler> _logger;
        private readonly IStatDevParser _statDevParser;

        public SurveyHandler(ILogger<SurveyHandler> logger,
                             IStatDevParser statDevParser)
        {
            _logger = logger;
            _statDevParser = statDevParser;
        }

        public int RunHandlerAndReturnExitCode(SurveyOptions options)
        {
            int exitCode = 0;

            FileAttributes attr = File.GetAttributes(options.FilePath);

            List<StatdevModel> statdevs = new();

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var xmlFiles = Directory.EnumerateFiles(options.FilePath, "*.xml");

                foreach (var item in xmlFiles)
                {
                    var xdoc = XDocument.Load(item);

                    statdevs.Add(_statDevParser.Parse(xdoc));
                }
            }
            else
            {
                var xdoc = XDocument.Load(options.FilePath);
                statdevs.Add(_statDevParser.Parse(xdoc));
            }

            SpreadsheetCreator.CreateFuelPosSurvey(statdevs, options.OutputPath);


            return exitCode;
        }
    }
}
