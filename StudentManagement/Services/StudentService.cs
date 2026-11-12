using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService
    {
        private readonly IDbContextFactory<StudentDbContext> _contextFactory;

        public StudentService(IDbContextFactory<StudentDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public enum DeleteStudentResult
        {
            Deleted,
            NotFound
        }

        public async Task AddStudent(Student student)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.LastName))
                {
                    throw new Exception("Name or lastname cannot be empthy.");
                }

                if (student.Midterm < 0 || student.Midterm >= 101)
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
        }

        public async Task<List<Student>> GetStudents()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Students.AsNoTracking().ToListAsync();
            }
        }

        public async Task<int> GetStudentsCount()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Students.CountAsync();
            }
        }

        public async Task<double?> GetClassAverage()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                double? sum = 0;
                foreach (var student in _context.Students)
                {
                    sum += student.Average;
                }
                double studentCount = _context.Students.Count();
                return sum / studentCount;
            }
        }
        public async Task DeleteStudent(int studentId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                foreach (var student in _context.Students)
                {
                    if (student.StudentId == studentId)
                    {
                        _context.Students.Remove(student);

                    }
                    else
                    {
                        //return DeleteStudentResult.NotFound;
                    }
                }

                await _context.SaveChangesAsync();
                //return DeleteStudentResult.Deleted;
            }
        }

        public async Task UpdateStudent(Student student)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                foreach (var record in _context.Students)
                {
                    if (record.StudentId == student.StudentId)
                    {
                        record.Name = student.Name;
                        record.Name = student.Name;
                        record.LastName = student.LastName;
                        record.Midterm = student.Midterm;
                        record.Final = student.Final;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> SearchStudent(string searchText)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

              return await _context.Students.Where(x => x.Name.Contains(searchText) ||  x.LastName.Contains(searchText)).OrderByDescending(x => x.Name).ToListAsync();
                

            }
        }

        public async Task<List<Student>> StudentSort(string chosenOption)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                if(chosenOption == "Name (A-Z)")
                {
                    return await _context.Students.OrderBy(x => x.Name).ToListAsync();

                }else if(chosenOption == "Name(Z-A)")
                {
                    return await _context.Students.OrderByDescending(x => x.Name).ToListAsync();

                }
                else if(chosenOption == "Average(Ascending)")
                {
                    return await _context.Students.OrderBy(x => (x.Midterm * 0.4 + x.Final * 0.6)).ToListAsync();


                }
                else if(chosenOption == "Average (Descending)")
                {
                    return await _context.Students.OrderByDescending(x => (x.Midterm * 0.4 + x.Final * 0.6)).ToListAsync();
                }
                else
                {
                    return await _context.Students.ToListAsync();
                }
            }
        }
    }
}
