using System;

namespace generic_api.Model.Auth
{
    public class RefreshToken
    {
        public string AppUserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpirationTimeUtc { get; private set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public bool Active => DateTime.UtcNow <= ExpirationTimeUtc;

        public RefreshToken(string token, DateTime expires)
        {
            Token = token;
            ExpirationTimeUtc = expires;
        }
    }
}