namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SurveyOptions
    {
        public SurveyOptions(string filepath, string output)
        {
            FilePath = filepath;
            OutputPath = output;
        }
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
    }
}
