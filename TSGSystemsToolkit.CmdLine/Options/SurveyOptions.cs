using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Options
{
    [Verb("survey", HelpText = "Tools for generating survey outputs based on one or more StatDev.xml files")]
    public class SurveyOptions
    {
        [Option('i', "input", HelpText = "Path to either an individual file, or directory containing" +
            " multiple StatDev XMLs")]
        public string InputPath { get; set; }

        [Option('o', "output", HelpText = "Path for any output files")]
        public string OutputPath { get; set; }
        
        [Option('s', "ssheet", HelpText = "Create a survey spreadsheet")]
        public bool CreateSurveySpreadsheet { get; set; }

        [Usage(ApplicationAlias = "systk")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                    new Example("Create a survey spreadsheet from a directory filled with StatDev XMLs",
                        new SurveyOptions {InputPath = "C:\\Path\\To Your\\Statdevs", CreateSurveySpreadsheet = true})
                };
            }
        }
    }
}
