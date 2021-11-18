using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface ITerminalsHandler
    {
        int RunHandlerAndReturnExitCode(TerminalsOptions options);
    }
}