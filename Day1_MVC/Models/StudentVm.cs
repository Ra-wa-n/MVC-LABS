using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class StudentVm
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 120, ErrorMessage = "Age must be valid")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    [Remote("CheckEmail","Student",AdditionalFields = "Id") ]
    public string Email { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public int deptno { get; set; }
    public List<int> CourseIds { get; set; } = new List<int>();
    
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
public class EditStd
{

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 120, ErrorMessage = "Age must be valid")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    [Remote("CheckEmail", "Student", AdditionalFields = "Id")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public int deptno { get; set; }
    public List<int> CourseIds { get; set; } = new List<int>();

}