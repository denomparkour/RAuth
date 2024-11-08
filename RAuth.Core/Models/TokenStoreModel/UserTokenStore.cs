using RAuth.Core.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAuth.Core.Models.TokenStoreModel
{
    public class UserTokenStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddDays(15);
    }
}
