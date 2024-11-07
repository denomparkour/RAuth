using Microsoft.AspNetCore.Identity;
using RAuth.Core.Models.AddressModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAuth.Core.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DateOfBirth { get; set; }
        public Address Address { get; set; }
        [ForeignKey("Address")]
        public Guid? AddressId { get; set; }
        public string ProfilePicture { get; set; }
    }
}
