namespace SysTk.WebAPI.GraphQL.Errors
{
    public class DebugParameterNotExistsError : Exception
    {
        public DebugParameterNotExistsError(string parameterName, string processName) : base($"Parameter {parameterName} for process {processName} does not exist.")
        {

        }
    }
}
