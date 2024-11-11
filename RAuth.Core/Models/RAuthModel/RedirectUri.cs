using RAuth.Core.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAuth.Core.Models.RAuthModel
{
    public class RedirectUri
    {
        public ApplicationUser User { get; set; }
        [Key]
        [ForeignKey("UserId")]
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
    }
}
