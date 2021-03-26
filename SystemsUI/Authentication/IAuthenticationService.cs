using System.Threading.Tasks;
using SystemsUI.Models;

namespace SystemsUI.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication);
        Task Logout();
    }
}