using System.ComponentModel.DataAnnotations;

namespace Day1_MVC.Models
{
    public class LoginVm
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
