using CommandLine;

namespace TSGSystemsToolkit.CmdLine.Options
{
    [Verb("create-mutation", HelpText = "Create mutations for FuelPOS")]
    public class CreateMutationOptions
    {
        [Option('c', "card-idenftifications", SetName = "crdid", HelpText = "Path to CardIdentifications.db3")]
        public string CardIdentification { get; set; }

        [Option('o', "output", HelpText = "Output directory - leave blank to create files in the same directory as the db3")]
        public string OutputPath { get; set; }
    }
}
