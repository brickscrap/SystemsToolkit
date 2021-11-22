using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface IUpdateHandler
    {
        int RunHandlerAndReturnExitCode(UpdateOptions options);
    }
}