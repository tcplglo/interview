using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace generic_api.Model.Extensions
{
    public static class AuthExtensions
    {
        public static class Roles
        {
            public const string requested = "requested";
            public const string tcpl = "tcpl";
            public const string tcpl_manager = "tcpl_manager";
            public const string tcpl_admin = "tcpl_admin";
            public const string application = "application";

            public static List<string> RoleNamesList()
            {
                var roles = typeof(Roles).GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
                return roles.Select(r => r.Name.Replace("_", " ")).ToList();
            }
        }

        public static class Claims
        {
            public const string FirstLogin = "FirstLogin";
            public const string AnyOtherClaim = "thisIsASample";

            public static List<string> ClaimNamesList()
            {
                var claim = typeof(Claims).GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
                return claim.Select(r => r.Name.Replace("_", " ")).ToList();
            }
        }

        public static class OpClaims
        {
            public const string AssignKey = "Assighn Key";
            public const string RotateKey = "Rotate Key";

            public static List<string> ClaimNamesList()
            {
                var claim = typeof(OpClaims).GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
                return claim.Select(r => r.Name.Replace("_", " ")).ToList();
            }
        }

        public static class Policies
        {
            public const string AdminAccess = "AdminAccess";
            public const string CanAssignKey = "CanAssignKey";
            public const string CanRotateKey = "CanRotateKey";

            public static List<string> PolicyNamesList()
            {
                var policies = typeof(Policies).GetFields(BindingFlags.Public | BindingFlags.Static).ToList();
                return policies.Select(r => r.Name.Replace("_", " ")).ToList();
            }
        }
    }
}
