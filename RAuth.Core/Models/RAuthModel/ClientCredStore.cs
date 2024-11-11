using RAuth.Core.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAuth.Core.Models.RAuthModel
{
    public class ClientCredStore
    {
        [Key]
        [ForeignKey("user")]
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<RedirectUri> RedirectUris { get; set; }
        public ClientUser user { get; set; }
    }
}
