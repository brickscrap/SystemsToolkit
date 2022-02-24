namespace TSGSystemsToolkit.CmdLine.Options
{
    public class SendFileOptions : OptionsBase
    {
        public string FilePath { get; set; }
        public string Cluster { get; set; }
        public string Target { get; set; }
        public string List { get; set; }
        public string Site { get; set; }
    }
}
