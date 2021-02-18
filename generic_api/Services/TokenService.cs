using generic_api.Model.Auth;
using generic_api.Model.ConfigurationSections;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace generic_api.Services
{
    public interface ITokenService
    {
        string GenerateToken(int size = 32);
        AccessToken GenerateEncodedToken(AppIdentity appIdentity);
    }

    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public AccessToken GenerateEncodedToken(AppIdentity appIdentity)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, appIdentity.UserName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, appIdentity.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
                }),
                Expires = _jwtSettings.ExpirationTimeUtc,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);

            return new AccessToken(tokenHandler.WriteToken(token), (DateTime)descriptor.Expires);

        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() 
                                                             - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                                             .TotalSeconds);
    }



}
