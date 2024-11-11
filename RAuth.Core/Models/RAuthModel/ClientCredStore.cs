using System.ComponentModel.DataAnnotations;

namespace RAuth.Core.Models.RAuthModel
{
    public class ClientCredStore
    {
        [Key]
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<RedirectUri> RedirectUris { get; set; }
    }
}
