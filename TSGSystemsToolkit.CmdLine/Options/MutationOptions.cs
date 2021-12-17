namespace TSGSystemsToolkit.CmdLine.Options
{
    public class MutationOptions : OptionsBase
    {
        public MutationOptions(string cardid, string output)
        {
            CardIdMutPath = cardid;
            OutputPath = output;
        }
        public string CardIdMutPath { get; set; }
        public string OutputPath { get; set; }
    }
}
