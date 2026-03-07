using System.ComponentModel.DataAnnotations;
using Project.Data.Model;

namespace Day1_MVC.Models
{
    public class DeptVm
    {
     
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public int  StudentCount { get; set; }
        public List<int> Crs { get; set; }
    }
}
