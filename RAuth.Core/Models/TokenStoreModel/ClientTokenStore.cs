using RAuth.Core.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RAuth.Core.Models.TokenStoreModel
{
    public class ClientTokenStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ClientUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddDays(7);
    }
}
