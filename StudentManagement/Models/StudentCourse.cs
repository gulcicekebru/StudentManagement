using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class StudentCourse
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Student Student { get; set; }= null!;
        public Course Course { get; set; }= null!;
    }
    
}
