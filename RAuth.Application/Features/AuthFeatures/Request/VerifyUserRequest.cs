using MediatR;
using RAuth.Application.DTO.AuthDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class VerifyUserRequest : IRequest<string>
    {
        public VerifyUserDTO verifyUser { get; set; }
    }
}
