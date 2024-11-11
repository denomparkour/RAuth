using MediatR;
using RAuth.Application.DTO.UserDTO;

namespace RAuth.Application.Features.UserFeatures.Request
{
    public class UpdateUserRequest : IRequest<string>
    {
        public string UserId { get; set; }
        public UpdateUserDTO updateUser { get; set; }

    }
}
