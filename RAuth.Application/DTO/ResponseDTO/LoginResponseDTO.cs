namespace RAuth.Application.DTO.ResponseDTO
{
    public class LoginResponseDTO
    {
        public string JWT { get; set; }
        public string? RefreshToken { get; set; }
    }
}
