using Day1_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data.Model;
using Project.Data.Repo;

namespace Day1_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
           
            var model = _unitOfWork.DepartmentRepo.GetAll(d=>d.Courses);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.crs = _unitOfWork.CourseRepo.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(DeptVm dept)
        {
            if (ModelState.IsValid)
            {
                var model = new Department
                {
                    Name = dept.Name,
                    StudentCount = dept.StudentCount,
                    Courses = new List<Course>()
                };

                if (dept.Crs != null)
                {
                    foreach (var courseId in dept.Crs)
                    {
                        
                        var existingCourse = _unitOfWork.CourseRepo.GetById(courseId);
                        if (existingCourse != null)
                        {
                            model.Courses.Add(existingCourse);
                        }
                    }
                }

                _unitOfWork.DepartmentRepo.Add(model);
                _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.crs = _unitOfWork.CourseRepo.GetAll();
            return View(dept);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();
            var model = _unitOfWork.DepartmentRepo.GetById(id.Value);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null) return BadRequest();
            _unitOfWork.DepartmentRepo.Delete(id.Value);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest();
            var model = _unitOfWork.DepartmentRepo.GetAll(d => d.Courses)
                            .FirstOrDefault(d => d.Id == id.Value);
            if (model == null) return NotFound();
           var allcrs = _unitOfWork.CourseRepo.GetAll();
            var crs = model.Courses.ToList(); 
            ViewBag.crsnotinpept= allcrs.Except(crs);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, Department dept, int[] coursestoadd, int[] coursestoremove)
        {
            
            var deptInDb = _unitOfWork.DepartmentRepo.GetAll(d => d.Courses)
                              .FirstOrDefault(d => d.Id == id);
            if (deptInDb == null) return NotFound();
            deptInDb.Name = dept.Name;
            deptInDb.StudentCount = dept.StudentCount;

            foreach (var courseId in coursestoremove)
            {
                Course c = deptInDb.Courses.FirstOrDefault(c => c.Id == courseId);
                if (c != null)
                    deptInDb.Courses.Remove(c);
            }
            foreach (var item in coursestoadd)
            {
                var course = _unitOfWork.CourseRepo.GetById(item);
                deptInDb.Courses.Add(course);
            }
          //  _unitOfWork.DepartmentRepo.Update(dept);

            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Grades(int? deptid, int? crsid)
        {
            if (deptid == null || crsid == null) return BadRequest();

            var course = _unitOfWork.CourseRepo.GetById(crsid.Value);
            var dept = _unitOfWork.DepartmentRepo.GetAll(d => d.Students)
              .FirstOrDefault(d => d.Id == deptid.Value);
            var students = dept?.Students?.ToList() ?? new List<Student>();
            var studentIds = students.Select(s => s.Id).ToList();

            var grades = _unitOfWork.StudentCourseRepo.FindAll(g =>
                g.CourseId == crsid.Value && studentIds.Contains(g.StudentId)).ToList();

            ViewBag.Course = course;
            ViewBag.Students = students;
            ViewBag.Grades = grades;
            return View();
        }

        [HttpPost]
        public IActionResult Grades(int deptid, int crsid, Dictionary<int, int> grades)
        {
            if (grades == null || !grades.Any()) return RedirectToAction(nameof(Index));

            foreach (var entry in grades)
            {
                var studentCourse = _unitOfWork.StudentCourseRepo
     .FindAll(sc => sc.StudentId == entry.Key && sc.CourseId == crsid)
     .FirstOrDefault();

                if (studentCourse != null)
                {
                    studentCourse.Degree = entry.Value;
                    _unitOfWork.StudentCourseRepo.Update(studentCourse);
                }
            }
            _unitOfWork.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }
    }
}