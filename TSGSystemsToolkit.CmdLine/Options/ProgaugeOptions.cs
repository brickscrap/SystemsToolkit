namespace TSGSystemsToolkit.CmdLine.Options
{
    public class ProgaugeOptions
    {
        public ProgaugeOptions(string filePath,
                               string output,
                               bool fuelPosFile)
        {
            FilePath = filePath;
            OutputPath = output;
            CreateFuelPosFile = fuelPosFile;
        }
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool CreateFuelPosFile { get; set; }
    }
}
