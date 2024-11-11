using RAuth.Core.Models.AddressModel;

namespace RAuth.Application.DTO.RAuthDTO
{
    public class GetRAuthUserResponseDTO
    {
        public DateOnly DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
