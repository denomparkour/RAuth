namespace RAuth.Application.DTO.RAuthDTO
{
    public class CreateRAuthResponseDTO
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string JWT {  get; set; }
        public string RefreshToken { get; set; }
    }
}
