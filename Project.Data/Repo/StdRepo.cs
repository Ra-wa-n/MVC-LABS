//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Project.Data.Data;
//using Project.Data.Model;
//using Microsoft.EntityFrameworkCore;

//namespace Project.Data.Repo
//{
//    public class StdRepo : IEntityRepo<Student>
//    {
//        public PContext context;
//        public StdRepo(PContext context)
//        {
//            this.context = context;
//        }
//        public void Add(Student entity)
//        {
//           context.Add(entity);
//            context.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            var std = context.Students.Find(id);
//            context.Remove(std);
//            context.SaveChanges();
//        }

//        public List<Student> GetAll()
//        {
//           return context.Students.Include(s => s.Department).ThenInclude(c=>c.Courses).ToList();
//        }

//        public Student GetById(int id)
//        {
//            return context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
//        }

//        public void Update(Student entity)
//        {
//            context.Update(entity);
//            context.SaveChanges();
//        }
//    }
//}
