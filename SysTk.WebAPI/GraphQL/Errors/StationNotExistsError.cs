namespace SysTk.WebAPI.GraphQL.Errors
{
    public class StationNotExistsError : Exception
    {
        public StationNotExistsError(string id) : base($"Station with ID {id} does not exist.")
        {
        }
    }
}
