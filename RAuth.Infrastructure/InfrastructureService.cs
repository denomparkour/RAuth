using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RAuth.Application.Repository;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;
using RAuth.Infrastructure.Repository;
using System.Security.Claims;
using System.Text;

namespace RAuth.Infrastructure
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("Db"));
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddIdentityCore<ApplicationUser>(options => options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager<SignInManager<ApplicationUser>>().AddDefaultTokenProviders();
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddCookie("Cookies").AddCookie(IdentityConstants.ExternalScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!))
                };
            }).AddGoogle("google", o =>
            {
                o.ClientId = configuration["OAuth:ClientId"]!;
                o.ClientSecret = configuration["OAuth:ClientSecret"]!;
                o.SaveTokens = true;
                o.TokenEndpoint = "https://oauth2.googleapis.com/token";
                o.CallbackPath = "/user/oauth/google";
                o.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
                o.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                o.Scope.Add("openid");
                o.Scope.Add("email");
                o.Scope.Add("profile");
                o.ClaimActions.MapJsonKey(ClaimTypes.Name, "name", ClaimValueTypes.String);
                o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id", ClaimValueTypes.String);
                o.ClaimActions.MapJsonKey("urn:google:picture", "picture", ClaimValueTypes.String);
                o.Events = new OAuthEvents
                {
                    OnTicketReceived = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        var user = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                        context.RunClaimActions(user.RootElement);
                    },
                    OnRemoteFailure = context =>
                    {
                        Console.WriteLine("OAuth Remote Failure:");
                        Console.WriteLine($"Failure: {context.Failure.Message}");
                        return Task.CompletedTask;
                    }

                };
            });
            
            return service;
        }
    }
}
