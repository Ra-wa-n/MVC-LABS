using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Data.Data;
using Project.Data.Model;
using Project.Data.Repo;


public class StudentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    [Authorize(Roles = "Student")]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);

        var student = _unitOfWork.StudentRepo
            .GetAll(s => s.Department)
            .FirstOrDefault(s => s.UserId == userId);
        var grades = _unitOfWork.StudentCourseRepo
            .GetAll(sc => sc.Course.Departments)
            .Where(sc => sc.StudentId == student.Id)
            .ToList();
        ViewBag.grades = grades;
        return View(student);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult DisplayStudents()
    {
       
        var students = _unitOfWork.StudentRepo.GetAll(s => s.Department).ToList();
        var studentCoursesMap = _unitOfWork.StudentCourseRepo
            .GetAll(sc => sc.Course.Departments) 
            .AsEnumerable()
            .Where(sc => {
                var student = students.FirstOrDefault(s => s.Id == sc.StudentId);
                if (student == null) return false;
                return sc.Course.Departments.Any(d => d.Id == student.DepartmentId);
            }).GroupBy(sc => sc.StudentId).ToDictionary(
                g => g.Key,
                g => g.Select(sc => sc.Course).ToList()
            );

        ViewBag.StudentCoursesMap = studentCoursesMap;
        return View(students);
    }
  
    public IActionResult Create()
    {
        var existingStudentUserIds = _unitOfWork.StudentRepo.GetAll().Select(s => s.UserId).ToList();

        var availableUsers = _userManager.GetUsersInRoleAsync("Student").Result
                             .Where(u => !existingStudentUserIds.Contains(u.Id));

        ViewBag.Users = new SelectList(availableUsers, "Id", "Email");
        ViewBag.Departments = new SelectList(_unitOfWork.DepartmentRepo.GetAll(), "Id", "Name");
        ViewBag.crs = new SelectList(_unitOfWork.CourseRepo.GetAll(), "Id", "Name");
        return View();
    }
    [HttpPost]

    public async Task<IActionResult> Create(StudentVm studentVm)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = studentVm.UserName, Email = studentVm.Email };
            var result = await _userManager.CreateAsync(user, studentVm.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");

                
                var student = new Student
                {
                    Name = studentVm.Name,
                    Age = studentVm.Age,
                    Email = studentVm.Email,
                    UserId = user.Id,
                    DepartmentId = studentVm.deptno
                };

                _unitOfWork.StudentRepo.Add(student);
                _unitOfWork.SaveChanges();
                var department = _unitOfWork.DepartmentRepo.GetById(student.DepartmentId.Value, "Courses");

                if (department?.Courses != null)
                {
                    foreach (var crs in department.Courses)
                    {
                        var stdcrs = new StudentCourse
                        {
                            StudentId = student.Id, 
                            CourseId = crs.Id
                        };
                        _unitOfWork.StudentCourseRepo.Add(stdcrs);
                    }
                    _unitOfWork.SaveChanges();
                }

                return RedirectToAction(nameof(DisplayStudents));
            }

            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }
        ViewBag.Departments = new SelectList(_unitOfWork.DepartmentRepo.GetAll(), "Id", "Name");
        return View(studentVm);
    }
    public IActionResult Edit(int? id)
    {
        if (id == null) return BadRequest();

        var model = _unitOfWork.StudentRepo.GetById(id.Value);
        if (model == null) return NotFound();

        ViewBag.depts = _unitOfWork.DepartmentRepo.GetAll();
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, Student std)
    {
        var studentFromDb = _unitOfWork.StudentRepo.GetById(id);

        studentFromDb.Name = std.Name;
        studentFromDb.Age = std.Age;
        studentFromDb.Email = std.Email;
        studentFromDb.DepartmentId = std.DepartmentId;

        _unitOfWork.StudentRepo.Update(studentFromDb);
        _unitOfWork.SaveChanges();

        return RedirectToAction(nameof(DisplayStudents));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return BadRequest();
        var model = _unitOfWork.StudentRepo.GetById(id.Value);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConmfirmed(int? id)
    {
        if (id == null) return BadRequest();
        _unitOfWork.StudentRepo.Delete(id.Value);
        _unitOfWork.SaveChanges();
        return RedirectToAction(nameof(DisplayStudents));
    }

    public IActionResult CheckEmail(string Email, int? id)
    {
        var existingStudent1 = _unitOfWork.StudentRepo.GetAll()
            .FirstOrDefault(s => s.Email == Email && (id == null || s.Id != id.Value));

        if (existingStudent1 != null)
        {
            return Json($"Email {Email} is already in use.");
        }

        return Json(true);
    }
}