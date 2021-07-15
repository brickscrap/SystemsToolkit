using System.Net.Http;
using System.Threading.Tasks;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace TSGSystemsToolkit.DesktopUI.Library.API
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string userName, string password);
        Task GetLoggedInUserInfo(string token);
        void LogOffUser();
    }
}