using Microsoft.Extensions.DependencyInjection;
using static generic_api.Model.Extensions.AuthExtensions;

namespace generic_api.Model.Auth
{
    static class InitializeAuthorization
    {
        internal static void RegisterAuthorizationHandlers(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.CanAssignKey, policy =>
                    policy.RequireAssertion(
                        context => context.User.IsInRole(Roles.tcpl)
                                   || context.User.IsInRole(Roles.tcpl_admin)
                                   || context.User.HasClaim(c => c.Type.Equals(OpClaims.AssignKey))));

                options.AddPolicy(Policies.CanRotateKey, policy =>
                    policy.RequireAssertion(
                        context => context.User.IsInRole(Roles.tcpl_admin)
                                   || context.User.HasClaim(c => c.Type.Equals(OpClaims.RotateKey))));

                options.AddPolicy(Policies.AdminAccess, policy =>
                    policy.RequireAssertion(
                        context => context.User.IsInRole(Roles.tcpl_admin)
                        && context.User.Identity.Name.Contains("@tcpl.io")
                    ));
            });
        }
    }
}