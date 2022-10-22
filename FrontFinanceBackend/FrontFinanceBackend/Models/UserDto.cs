using System.ComponentModel.DataAnnotations;

namespace FrontFinanceBackend.Models
{
    public class UserDto
    {
        [Key]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string? LastName { get; set; }
        public UserDto()
        {

        }
        public UserDto(FrontUser user)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
        }
    }
}