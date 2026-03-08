//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Project.Data.Data;
//using Project.Data.Model;

//namespace Project.Data.Repo
//{
//    public class CrsRepo : IEntityRepo<Course>
//    {
//        public PContext context ;
//        public CrsRepo(PContext context)
//        {
//            this.context = context;
//        }
//        public void Add(Course entity)
//        {
//           context.Add(entity);
//            context.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//           context.Remove(context.Courses.Find(id));
//            context.SaveChanges();
//        }

//        public List<Course> GetAll()
//        {
//           return context.Courses.ToList();
//        }

//        public Course GetById(int id)
//        {
//            return context.Courses.Find(id);
//        }

//        public void Update(Course entity)
//        {
//          context.Update(entity);
//            context.SaveChanges();
//        }
//        public void Attach(Course entity)
//        {
//            context.Courses.Attach(entity);
//        }
//    }
//}
