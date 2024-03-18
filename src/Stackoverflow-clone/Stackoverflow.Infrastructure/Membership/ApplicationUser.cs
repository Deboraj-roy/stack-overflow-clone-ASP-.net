using Microsoft.AspNetCore.Identity;
using System;

namespace Stackoverflow.Infrastructure.Membership
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public int? Reputation { get; set; }
        public DateTime? RegistrationDate { get; set; } = DateTime.UtcNow;
        public string? UserType { get; set; } = UserRoles.Newbie;
    }
}
