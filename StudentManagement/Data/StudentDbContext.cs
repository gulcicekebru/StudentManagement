using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
namespace StudentManagement.Data

{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        //fluent api ile iki tane pk key i burada tanımladık

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasKey(x => new {x.StudentId, x.CourseId});
        }

    }
}
