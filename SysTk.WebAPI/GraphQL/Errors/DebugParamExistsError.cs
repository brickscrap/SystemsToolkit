namespace SysTk.WebAPI.GraphQL.Errors
{
    public class DebugParamExistsError : Exception
    {
        public DebugParamExistsError(string name, int processId) : base($"Debug parameter: {name} already exists for process with ID {processId}.")
        {
        }
    }
}
