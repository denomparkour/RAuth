using RAuth.Core.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAuth.Core.Models.RAuthModel
{
    public class RedirectUri
    {
        public ApplicationUser User { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
