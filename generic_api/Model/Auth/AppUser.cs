using System.Collections.Generic;

namespace generic_api.Model.Auth
{
    public class AppUser
    {
        public string id { get; set; }
        public string FriendlyName { get; set; }
        public string Exceprt { get; set; }

        public string AccessToken { get; set; }
        public List<RefreshToken> RefreshTokens {get;set;}
    }
}
