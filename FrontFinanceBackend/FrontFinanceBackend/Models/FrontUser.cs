using Microsoft.AspNetCore.Identity;

namespace FrontFinanceBackend.Models
{
    public class FrontUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}