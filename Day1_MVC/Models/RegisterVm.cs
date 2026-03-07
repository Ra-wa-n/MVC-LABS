using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Day1_MVC.Models
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Username is required")]
        public string  UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Remote("CheckEmail", "Account", AdditionalFields = "UserName")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

    }
}
