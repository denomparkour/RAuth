namespace RAuth.Application.DTO.RAuthDTO
{
    public class RAuthTransRequest
    {
        public string RequesterId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddMinutes(1); 
    }
}
