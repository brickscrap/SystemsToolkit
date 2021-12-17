namespace TSGSystemsToolkit.CmdLine.Options
{
    public class TerminalsOptions : OptionsBase
    {
        public TerminalsOptions(string filepath, bool emisfile, string output)
        {
            FilePath = filepath;
            OutputPath = output;
            CreateEmisFile = emisfile;
        }
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool CreateEmisFile { get; set; }
    }
}
