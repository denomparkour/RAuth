using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RAuth.Core.Models.AddressModel;
using RAuth.Core.Models.OtpModel;
using RAuth.Core.Models.TokenStoreModel;
using RAuth.Core.Models.User;

namespace RAuth.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<OTP> Otp { get; set; }
        public DbSet<UserTokenStore> UserTokenStore { get; set; }
    }
}
