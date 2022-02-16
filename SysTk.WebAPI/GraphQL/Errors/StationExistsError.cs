using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Errors
{
    public class StationExistsError : Exception
    {
        public string Message { get; set; }

        public StationExistsError(string id, string ip, string existingIp = default, string existingId = default)
        {
            if (existingId == id)
                Message = $"Station with ID {id} already exists with IP {existingIp}.";

            if (existingIp == ip)
                Message = $"Station with IP {ip} already exists with station ID {existingId}.";
        }
    }
}
