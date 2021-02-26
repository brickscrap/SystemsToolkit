using FuelPOSToolkitApi.Models;
using FuelPOSToolkitDataManager.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using FuelPOSToolkitDataManager.Library.DataAccess;
using Microsoft.AspNetCore.Authorization;

namespace FuelPOSToolkitApi.Controllers
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
