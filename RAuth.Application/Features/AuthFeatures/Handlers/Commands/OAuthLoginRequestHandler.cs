using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Core.Models.User;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Commands
{
    public class OAuthLoginRequestHandler(SignInManager<ApplicationUser> signInManager) : IRequestHandler<OAuthLoginRequest, AuthenticationProperties>
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        public async Task<AuthenticationProperties> Handle(OAuthLoginRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_signInManager.ConfigureExternalAuthenticationProperties(IdentityConstants.ExternalScheme, "/auth/login/google/handler"));
        }
    }
}
