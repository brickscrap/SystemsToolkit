using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystems.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserData _userData;

        public UserController(UserManager<IdentityUser> userManager, IUserData userData)
        {
            _userManager = userManager;
            _userData = userData;
        }

        [HttpGet]
        public async Task<UserModel> GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var output = await _userData.GetUserById(userId);

            return output.FirstOrDefault();
        }
    }
}
