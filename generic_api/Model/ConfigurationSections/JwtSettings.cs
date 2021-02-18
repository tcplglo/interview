using System;

namespace generic_api.Model.ConfigurationSections
{
    public class JwtSettings
    {
        //public string Audience { get; set; }
        //public string Issuer { get; set; }
        public string Secret{ get; set; }
        public int Minutes { get; set; }

        public DateTime ExpirationTimeUtc => DateTime.UtcNow.AddMinutes(Minutes);

    }
}
