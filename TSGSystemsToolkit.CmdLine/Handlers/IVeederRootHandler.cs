using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface IVeederRootHandler
    {
        int RunHandlerAndReturnExitCode(VeederRootOptions options);
    }
}