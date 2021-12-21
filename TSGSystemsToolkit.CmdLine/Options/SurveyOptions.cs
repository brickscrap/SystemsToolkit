namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SurveyOptions : OptionsBase
    {
        public SurveyOptions(string filepath, string output, bool fuelpos, bool serialnumbers)
        {
            FilePath = filepath;
            OutputPath = output;
            FuelPosSurvey = fuelpos;
            SerialNumberSurvey = serialnumbers;
        }
        public string FilePath { get; set; }
        public string OutputPath { get; set; }
        public bool FuelPosSurvey { get; set; }
        public bool SerialNumberSurvey { get; set; }
    }
}
