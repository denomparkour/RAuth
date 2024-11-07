using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RAuth.Infrastructure.Data;

namespace RAuth.Infrastructure
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("Db"));
            return service;
        }
    }
}
