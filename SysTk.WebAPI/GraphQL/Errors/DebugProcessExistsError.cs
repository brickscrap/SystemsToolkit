namespace SysTk.WebAPI.GraphQL.Errors
{
    public class DebugProcessExistsError : Exception
    {
        public DebugProcessExistsError(string processName) : base($"Debug process with name {processName} already exists.")
        {

        }
    }
}
