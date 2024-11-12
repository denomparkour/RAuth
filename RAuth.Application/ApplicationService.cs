using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RAuth.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("SignalRPolicy",
                    policy =>
                    {
                        policy.WithOrigins("https://127.0.0.1:5500")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .SetIsOriginAllowed(origin => true);
                    });
            });
            return services;
        }
    }
}
