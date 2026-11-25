using Microsoft.EntityFrameworkCore;
using StudentManagement.Components.Pages;
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
                    throw new Exception("Name or lastname cannot be empty.");
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

        public async Task<double?> GetClassAverage(List<Student> updatedList)
        {
                double? sum = 0;
                foreach (var student in updatedList)
                {
                    sum += student.Average;
                }
                double studentCount = updatedList.Count();

                if(studentCount == 0)
                {
                    throw new Exception("Student count might be zero");
                }
                return sum / studentCount;

        }

        public async Task DeleteStudent(int studentId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);

                if(student != null)
                {
                    _context.Students.Remove(student);
                }
                await _context.SaveChangesAsync();
                //return DeleteStudentResult.Deleted;
            }
        }
        public async Task<Student> GetStudentById(int? studentId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var student = _context.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                return student;
            }
        }

        public async Task UpdateStudent(Student student)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var selectedStudent = _context.Students.FirstOrDefault(x => x.StudentId == student.StudentId);

                if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.LastName))
                {
                    throw new Exception("Name or lastname cannot be empty.");
                }

                if (student.Midterm < 0 || student.Midterm >= 101)
                {
                    throw new Exception("Midterm value must be in range of 0-100");
                }
                if (student.Final < 0 || student.Final >= 101)
                {
                    throw new Exception("Final value must be in range of 0-100");
                }

                if (selectedStudent != null)
                {
                    selectedStudent.Name = student.Name;
                    selectedStudent.LastName = student.LastName;
                    selectedStudent.Midterm = student.Midterm;
                    selectedStudent.Final = student.Final;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResult<Student>> FilteringProcesses(string searchText, string filterOption, string passedStatus, int pageNumber, int pageSize)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var tempList = _context.Students.AsQueryable();

                if (pageNumber <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero");
                }
                if (pageSize <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero");
                }

                // arama dolu mu?
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    tempList = tempList.Where(x => x.Name.Contains(searchText) || x.LastName.Contains(searchText));
                }
                //state (passed, not passed) seçildi mi?
                if (!string.IsNullOrWhiteSpace(passedStatus))
                {
                    // tempList = tempList.Where(x => x.Status == passedStatus);
                    // status sınıf içerisinde midterm ve final değerlerinden hesaplandığından burada
                    // sql üzerindeki bilgilerden erişmek lazım yani o matematiği tekrar yapmak gerek.

                    tempList = tempList.Where(x => (x.Midterm * 0.4 + x.Final * 0.6 >= 50 ? "Passed" : "Not Passed") == passedStatus);

                }
                // sıralama seçildi mi?
                if (!string.IsNullOrWhiteSpace(filterOption))
                {
                    
                    switch (filterOption)
                    {
                        case "Name0":
                            tempList = tempList.OrderBy(x => x.Name);
                            break;
                        case "Name1":
                            tempList = tempList.OrderByDescending(x => x.Name);
                            break;
                        case "LastName0":
                            tempList = tempList.OrderBy(x => x.LastName);
                            break;
                        case "LastName1":
                            tempList = tempList.OrderByDescending(x => x.LastName);
                            break;
                        case "Midterm1":
                            tempList = tempList.OrderBy(x => x.Midterm);
                            break;
                        case "Midterm0":
                            tempList = tempList.OrderByDescending(x => x.Midterm);
                            break;
                        case "Final1":
                            tempList = tempList.OrderBy(x => x.Final);
                            tempList = tempList.OrderBy(x => x.Final);
                            break;
                        case "Final0":
                            tempList = tempList.OrderByDescending(x => x.Final);
                            break;
                        case "Average1":
                            tempList = tempList.OrderBy(x => (x.Midterm * 0.4 + x.Final * 0.6));
                            break;
                        case "Average0":
                            tempList = tempList.OrderByDescending(x => (x.Midterm * 0.4 + x.Final * 0.6));
                            break;
                        case "Is Passed0":
                            tempList = tempList.OrderBy(x => (x.Midterm * 0.4 + x.Final * 0.6));
                            break;
                        case "Is Passed1":
                            tempList = tempList.OrderByDescending(x => (x.Midterm * 0.4 + x.Final * 0.6));
                            break;
                    }
                    
                }

                int totalStudents = await tempList.CountAsync();

                int totalPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalStudents / pageSize));

                var students = await tempList.AsNoTracking()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToListAsync();

                return new PaginatedResult<Student>(students, totalStudents, totalPageNumber, pageNumber, pageSize);

            }
        }

    }
}
