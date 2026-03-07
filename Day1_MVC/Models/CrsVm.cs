using System.ComponentModel.DataAnnotations;

namespace Day1_MVC.Models
{
    public class CrsVm
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
