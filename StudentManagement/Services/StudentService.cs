using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService
    {
        private List<Student> students = new();

        public void AddStudent(Student student)
        {
            foreach(var registered in students)
            {
                if (registered.StudentId == student.StudentId)
                {
                    throw new Exception("Student Id is already taken!");
                }
            }

            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.LastName))
            {
                throw new Exception("Name or lastname cannot be empthy.");
            }

            if (student.Midterm<0 || student.Midterm >= 101)
            {
                throw new Exception("Midterm value must be in range of 0-100");
            }
            if (student.Final < 0 || student.Midterm >= 101)
            {
                throw new Exception("Final value must be in range of 0-100");
            }

            students.Add(student);
        }

        public List<Student> GetStudents()
        {
            return students;
        }
    }
}
