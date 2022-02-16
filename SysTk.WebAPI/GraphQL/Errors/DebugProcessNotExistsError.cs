namespace SysTk.WebAPI.GraphQL.Errors
{
    public class DebugProcessNotExistsError : Exception
    {
        public DebugProcessNotExistsError(string name) : base($"Debug process with name: {name}, does not exist.")
        {
        }

        public DebugProcessNotExistsError(int id) : base($"Debug process with ID: {id}, does not exist.")
        {
        }
    }
}
