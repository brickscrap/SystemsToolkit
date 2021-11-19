namespace TSGSystemsToolkit.CmdLine.Options
{
    public class CreateMutationOptions
    {
        public CreateMutationOptions(string cardid, string output)
        {
            CardIdMutPath = cardid;
            OutputPath = output;
        }
        public string CardIdMutPath { get; set; }
        public string OutputPath { get; set; }
    }
}
