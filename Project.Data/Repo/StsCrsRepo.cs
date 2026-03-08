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
//    public class StsCrsRepo : IEntityRepo<StudentCourse>
//    {
//        public PContext context;
//        public StsCrsRepo(PContext context)
//        {
//            this.context = context;
//        }
//        public void Add(StudentCourse entity)
//        {
//            context.Add(entity);
//            context.SaveChanges();

//        }

//        public void Delete(int id)
//        {
//            context.Remove(context.StudentCourses.Find(id));
//            context.SaveChanges();
//        }

//        public List<StudentCourse> GetAll()
//        {
//            return context.StudentCourses.Include(s => s.Student).ThenInclude(d => d.Department).ToList();
//        }

//        public StudentCourse GetById(int id)
//        {
//            return context.StudentCourses.Include(s => s.Student).ThenInclude(d => d.Department).FirstOrDefault(s => s.StudentId == id);
//        }

//        public void Update(StudentCourse entity)
//        {
//            context.Update(entity);
//            context.SaveChanges();
//        }
//        public List<Course> GetStudentCourses(int studentId)
//        {
//            return context.StudentCourses
//                .Where(sc => sc.StudentId == studentId)
//                .Include(sc => sc.Course)
//                .Select(sc => sc.Course)
//                .ToList();

//        }
//        public Dictionary<int, int?> GetGradesByCourse(int courseId, List<int> studentIds)
//        {
//            return context.StudentCourses
//                          .Where(sc => sc.CourseId == courseId && studentIds.Contains(sc.StudentId))
//                          .ToDictionary(sc => sc.StudentId, sc => sc.Degree);
//        }

//    }
//}

