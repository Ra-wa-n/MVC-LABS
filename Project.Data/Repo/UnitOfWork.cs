using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.Data;
using Project.Data.Model;

namespace Project.Data.Repo
{
    public interface IUnitOfWork
    {
        public EntityRepo<Course> CourseRepo { get; }
        public EntityRepo<Department> DepartmentRepo { get; }
        public EntityRepo<Student> StudentRepo { get; }
        public EntityRepo<Admin> AdminRepo { get; }
        public EntityRepo<StudentCourse> StudentCourseRepo { get; }
        int SaveChanges();
    }
    public class UnitOfWork : IUnitOfWork
    {
        public EntityRepo<Course> CourseRepo { get; }
        public EntityRepo<Department> DepartmentRepo { get; }
        public EntityRepo<Student> StudentRepo { get; }
        public EntityRepo<Admin> AdminRepo { get; }
        public EntityRepo<StudentCourse> StudentCourseRepo { get; }

        PContext context;
        public UnitOfWork(PContext pContext)
        {
            context = pContext;
            CourseRepo = new EntityRepo<Course>(context);
            DepartmentRepo = new EntityRepo<Department>(context);
            StudentRepo = new EntityRepo<Student>(context);
            AdminRepo = new EntityRepo<Admin>(context);
            StudentCourseRepo = new EntityRepo<StudentCourse>(context);

        }

        public int SaveChanges()
        {
         return  context.SaveChanges();
        }
    }
}
