using generic_api.Model.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace generic_api.Data
{
    public class ApplicationAuthDataContext : IdentityDbContext<AppIdentity>
    {
        public ApplicationAuthDataContext(DbContextOptions<ApplicationAuthDataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
