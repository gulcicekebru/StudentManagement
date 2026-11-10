using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService
    {
        private readonly StudentDbContext _context;

        public StudentService(StudentDbContext context)
        {
            _context = context;
        }
      
        public enum DeleteStudentResult
        {
            Deleted,
            NotFound
        }

        public async Task AddStudent(Student student)
        {        

            if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.LastName))
            {
                throw new Exception("Name or lastname cannot be empthy.");
            }

            if (student.Midterm<0 || student.Midterm >= 101)
            {
                throw new Exception("Midterm value must be in range of 0-100");
            }
            if (student.Final < 0 || student.Final >= 101)
            {
                throw new Exception("Final value must be in range of 0-100");
            }

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<int> GetStudentsCount()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<double> GetClassAverage()
        {
            double sum = 0;
            foreach(var student in _context.Students){
                sum += student.Average;               
            }
            double studentCount = _context.Students.Count();
            return sum / studentCount;
        }
        public async void DeleteStudent(int studentId)
        {
            foreach(var student in _context.Students)
            {
                if(student.StudentId == studentId)
                {
                    _context.Students.Remove(student);

                }
            }

            await _context.SaveChangesAsync();
        }

        public async void UpdateStudent(Student student)
        {
            foreach(var record in _context.Students)
            {
                if(record.StudentId == student.StudentId)
                {
                    record.Name = student.Name;
                    record.LastName = student.LastName;
                    record.Midterm = student.Midterm;
                    record.Final = student.Final;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
