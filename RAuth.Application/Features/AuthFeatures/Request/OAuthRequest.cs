using MediatR;
using RAuth.Application.DTO.ResponseDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class OAuthRequest : IRequest<LoginResponseDTO>
    {
    }
}
