using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RAuth.Application.Repository;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;
using RAuth.Infrastructure.Repository;

namespace RAuth.Infrastructure
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("Db"));
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddIdentityCore<ApplicationUser>(options => options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<ApplicationDbContext>();
            return service;
        }
    }
}
