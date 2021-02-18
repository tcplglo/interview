using System;

namespace generic_api.Model.Auth
{
    public sealed class AccessToken
    {
        public string Token { get; }
        public DateTime ExpirationTimeUtc { get; set; }

        public AccessToken(string token, DateTime expirationTime)
        {
            Token = token;
            ExpirationTimeUtc = expirationTime;
        }
    }
}