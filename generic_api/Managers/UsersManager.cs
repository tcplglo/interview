using generic_api.Data;
using generic_api.Model.Auth;
using generic_api.Model.Messages.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static generic_api.Model.Extensions.AuthExtensions;

namespace generic_api.Managers
{
    public interface IUsersManager
    {
        Task<AppIdentity> FindByNameAsync(string applicationName);
        Task<bool> CheckPasswordAsync(AppIdentity application, string apiKey);
        Task<RegistrationResponse> CreateAsync(AppIdentity user, string pwd, string description);
        AppUser GetUserFromIdentity(AppIdentity appIdentity);
        void AddRefreshTokentoUser(AppUser appUser, string refreshToken);
    }

    public class UsersManager : IUsersManager
    {
        private readonly UserManager<AppIdentity> _userManager;
        private readonly IUserRepository _userRepository;

        public UsersManager(
            UserManager<AppIdentity> userManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<AppIdentity> FindByNameAsync(string applicationName)
        {
            return await _userManager.FindByNameAsync(applicationName);
        }

        public async Task<bool> CheckPasswordAsync(AppIdentity application, string apiKey)
        {
            return await _userManager.CheckPasswordAsync(application, apiKey);
        }

        public async Task<RegistrationResponse> CreateAsync(AppIdentity user, string pwd, string description)
        {
            var result = await _userManager.CreateAsync(user, pwd);
            var response = new RegistrationResponse { Success = result.Succeeded };

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.requested);
                await _userManager.AddClaimAsync(user, new Claim(Claims.FirstLogin, pwd));

                _userRepository.CreateAppUser(user, description);
                
                response.Message = pwd;
            }
            else
            {
                response.Success = false;
            }

            return response;
        }

        public AppUser GetUserFromIdentity(AppIdentity appIdentity)
        {
            return _userRepository.GetAppUserByAppIdentityId(appIdentity);
        }

        public void AddRefreshTokentoUser(AppUser appUser, string refreshToken)
        {
            _userRepository.RemoveExpiredTokensForUser(appUser);
            _userRepository.AddRefreshtokenToUser(appUser, refreshToken);
        }
    }

    public class NoSqlUsersManager : IUsersManager
    {
        public async Task<AppIdentity> FindByNameAsync(string applicationName)
        {
            return new AppIdentity() { 
                UserName = "demo" 
            };
        }

        public async Task<bool> CheckPasswordAsync(AppIdentity application, string apiKey)
        {
            return true;
        }

        public async Task<RegistrationResponse> CreateAsync(AppIdentity user, string pwd, string description)
        {
            throw new NotImplementedException();
        }

        public AppUser GetUserFromIdentity(AppIdentity appIdentity)
        {
            throw new NotImplementedException();
        }

        public void AddRefreshTokentoUser(AppUser appUser, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
