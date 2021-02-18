using System.Collections.Generic;
using System.Linq;

namespace generic_api.Model.ConfigurationSections
{
    public class IpSecuritySettings
    {
        public bool FDIdEnabled { get; set; }
        public string AzureFrontDoorId { get; set; }
        public bool IpCheckEnabled { get; set; }
        public string AllowedIPs { get; set; }

        public List<string> GetAllowedIPsList()
        {
            return !string.IsNullOrEmpty(AllowedIPs) ? AllowedIPs.Split(';').ToList() : new List<string>();
        }
    }
}
