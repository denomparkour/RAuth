namespace RAuth.Application.DTO.AuthDTO
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
