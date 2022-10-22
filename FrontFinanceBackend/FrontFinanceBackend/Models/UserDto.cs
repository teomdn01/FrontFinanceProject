﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipBackend.Data
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
    }
}