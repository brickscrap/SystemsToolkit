using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public interface ISurveyHandler
    {
        int RunHandlerAndReturnExitCode(SurveyOptions options);
    }
}