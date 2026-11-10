using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
namespace StudentManagement.Data

{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        
    }
}
