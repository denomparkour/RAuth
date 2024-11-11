using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RAuth.Core.Models.AddressModel;
using RAuth.Core.Models.OtpModel;
using RAuth.Core.Models.RAuthModel;
using RAuth.Core.Models.TokenStoreModel;
using RAuth.Core.Models.User;

namespace RAuth.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Address> Address { get; set; }
        public DbSet<OTP> Otp { get; set; }
        public DbSet<UserTokenStore> UserTokenStore { get; set; }
        public DbSet<RedirectUri> RedirectUri { get; set; }
        public DbSet<ClientCredStore> ClientCredStore { get; set; }
        public DbSet<ClientUser> ClientUser { get; set; }
        public DbSet<ClientTokenStore> ClientTokenStore { get; set; }
    }
}
