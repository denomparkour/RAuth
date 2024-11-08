using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class OAuthLoginRequest : IRequest<AuthenticationProperties>
    {
    }
}
