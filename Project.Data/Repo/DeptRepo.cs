//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Project.Data.Data;
//using Project.Data.Model;

//namespace Project.Data.Repo
//{
//    public class DeptRepo : IEntityRepo<Department>
//    {
//        public PContext context;
//        public DeptRepo(PContext context)
//        {
//            this.context = context;
//        }
//        public void Add(Department entity)
//        {
//         context.Add(entity);
//            context.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            context.Remove(context.Departments.Find(id));
//            context.SaveChanges();
//        }

//        public List<Department> GetAll()
//        {
//            return context.Departments.Include(d=> d.Courses).ToList();    
//        }

//        public Department GetById(int id)
//        {
//            return context.Departments
//          .Include(d => d.Students)
//          .FirstOrDefault(d => d.Id == id);
        
//        }

//        public void Update(Department entity)
//        {
//           context.Update(entity);
//            context.SaveChanges();
//        }
//        public List<Course> GetDepartmentCourses(int departmentId)
//        {
//            return context.Departments
//                .Where(d => d.Id == departmentId)
//                .Include(d => d.Courses)
//                .SelectMany(d => d.Courses)
//                .ToList();
//        }
//    }
//}
