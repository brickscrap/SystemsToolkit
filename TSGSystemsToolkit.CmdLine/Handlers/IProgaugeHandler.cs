using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface IProgaugeHandler
    {
        int RunHandlerAndReturnExitCode(ProgaugeOptions options);
    }
}