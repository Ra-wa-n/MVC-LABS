using Day1_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
using Project.Data.Model;
using Project.Data.Repo;

namespace Day1_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {

            UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = UnitOfWork.CourseRepo.GetAll();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CrsVm Crs)
        {
            var vm = new Course
            {
                Name = Crs.Name,
            };
            UnitOfWork.CourseRepo.Add(vm);
            UnitOfWork.SaveChanges();   
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = UnitOfWork.CourseRepo.GetById(id.Value);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [HttpPost]
        public IActionResult EDit(int id ,Course crs)
        {
            {
                if (id != crs.Id)
                    return BadRequest();

                crs.Id = id;

                UnitOfWork.CourseRepo.Update(crs);
                UnitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

        }
        public IActionResult Delete(int id)
        {
            var model = UnitOfWork.CourseRepo.GetById(id);
            return View(model); 
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
                if (id == null)
                    return BadRequest();
                UnitOfWork.CourseRepo.Delete(id.Value);
            UnitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
