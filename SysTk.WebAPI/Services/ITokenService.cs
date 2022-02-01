using Microsoft.AspNetCore.Identity;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebAPI.Services
{
    public interface ITokenService
    {
        Task<dynamic> GenerateToken(string username);
        Task<bool> IsValidUsernameAndPassword(string userName, string password);
    }
}