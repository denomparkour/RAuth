namespace RAuth.Application.DTO.UserDTO
{
    public class UpdateUserDTO
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public UpdateAddressDTO Address { get; set; }
    }
}
