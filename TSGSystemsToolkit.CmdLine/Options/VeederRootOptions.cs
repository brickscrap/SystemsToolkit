namespace TSGSystemsToolkit.CmdLine.Options
{
    public class VeederRootOptions : OptionsBase
    {
        public VeederRootOptions(string filePath, string? output, bool fuelPosFile, bool csv)
        {
            FilePath = filePath;
            OutputPath = output;
            CreateFuelPosFile = fuelPosFile;
            CreateCsv = csv;
        }

        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool CreateFuelPosFile { get; set; }
        public bool CreateCsv { get; set; }
    }
}
