using Microsoft.AspNetCore.Identity;

namespace RAuth.Core.Models.User
{
    public class ClientUser : IdentityUser
    {
        public string OrganizationName { get; set; }
        public string ProfilePicture { get; set; }

    }
}
