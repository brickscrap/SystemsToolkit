using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface IMutationHandler
    {
        int RunHandlerAndReturnExitCode(CreateMutationOptions options);
    }
}