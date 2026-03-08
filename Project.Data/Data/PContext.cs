using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Data.Model;

namespace Project.Data.Data

{
    public class ApplicationUser : IdentityUser
    {
       public Student Student { get; set; }
        public Admin Admin { get; set; }
    }
    public class PContext: IdentityDbContext<ApplicationUser>
    {
     
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
       public DbSet<StudentCourse> StudentCourses { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public PContext()
        {

        }
        public PContext(DbContextOptions<PContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.;Database=MVC;Trusted_Connection=True; Trust Server Certificate=True");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>(IdentityRole =>
            {
                IdentityRole.HasData(
                    new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "Admin" },
                    new IdentityRole { Id = "2", Name = "Student", NormalizedName = "Student" }
                );
            });
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });
            base.OnModelCreating(modelBuilder);
            #region user table
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Roles)
            //    .WithMany(r => r.Users)
            //    .UsingEntity(j => j.ToTable("UserRole"));
            #endregion
            base.OnModelCreating(modelBuilder);

        }
    }
}
