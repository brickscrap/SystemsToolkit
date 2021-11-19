namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SurveyOptions
    {
        public SurveyOptions(string filepath, string output, bool spreadsheet)
        {
            FilePath = filepath;
            OutputPath = output;
            CreateSpreadsheet = spreadsheet;
        }
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool CreateSpreadsheet { get; set; }
    }
}
