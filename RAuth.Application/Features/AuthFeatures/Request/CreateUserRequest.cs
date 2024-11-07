using MediatR;
using RAuth.Application.DTO.AuthDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class CreateUserRequest  : IRequest<string>
    {
        public CreateUserDTO createUser { get; set; }
    }
}
