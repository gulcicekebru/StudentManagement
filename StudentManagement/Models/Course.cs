using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId{ get; set; }
        public  string CourseName { get; set; } = string.Empty;
        public int Credit { get; set; }
        public List<Student> RegisteredStudents { get; set; } = new List<Student>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        public Course(string courseName,int credit)
        {
            this.CourseName = courseName;
            this.Credit = credit;
        }

        public Course() { }


    }

    
}
