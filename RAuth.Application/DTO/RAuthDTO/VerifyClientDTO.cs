namespace RAuth.Application.DTO.RAuthDTO
{
    public class VerifyClientDTO
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public DateTime? ExpiryTime { get; set; }
    }
}
