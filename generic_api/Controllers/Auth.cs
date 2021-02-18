using generic_api.Managers;
using generic_api.Model.Auth;
using generic_api.Model.Extensions;
using generic_api.Model.Messages.Responses;
using generic_api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace generic_api.Controllers
{
    public class Auth : Controller
    {
        private readonly IUsersManager _usersManager;
        private readonly ITokenService _tokenService;

        public Auth(
            IUsersManager usersManager,
            ITokenService tokenSErvice)
        {
            _usersManager = usersManager;
            _tokenService = tokenSErvice;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string applicationName,[FromHeader] string apiKey)
        {
            var result = new LoginResponse() { Success = true };

            var appIdentity = await _usersManager.FindByNameAsync(applicationName);
            var appUser = _usersManager.GetUserFromIdentity(appIdentity);

            if(appUser != null)
            {
                if (await _usersManager.CheckPasswordAsync(appIdentity, apiKey))
                {
                    // generate refresh token
                    var refreshToken = _tokenService.GenerateToken();
                    _usersManager.AddRefreshTokentoUser(appUser, refreshToken);
                    result.RefreshToken = refreshToken;

                    // generate access token
                    AccessToken accessToken = _tokenService.GenerateEncodedToken(appIdentity);
                    result.AccessToken = accessToken;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Something went wrong is not a good result message... or is it?";
                }
            }


            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromQuery] string name, [FromQuery] string description, [FromQuery] string email )
        {
            var user = new AppIdentity { UserName = name, Email = email, SecurityStamp = Guid.NewGuid().ToString() };
            var pwd = PwdExtensions.GenerateRandomPassword();

            var result = await _usersManager.CreateAsync(user, pwd, description);
            
            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        /// <param name="accessToken">A valid token. Can be Expired.</param>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refesh([FromHeader]string accessToken, [FromHeader] string refreshToken)
        {
            return Ok();
        }

    }

}
