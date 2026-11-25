using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Data;
using StudentManagement.Models;


namespace StudentManagement.Services
{
    public class CourseService
    {
        private readonly IDbContextFactory<StudentDbContext> _contextFactory;

        private readonly List<Course> courseList;

        public CourseService(IDbContextFactory<StudentDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task InitializeCoursesAsync()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                if (await _context.Courses.AnyAsync())
                    return;

                var courseList = new List<Course>
            {
                    new Course("Math",5),
                    new Course("Computer Network",4),
                    new Course("Algorithms",4),
                    new Course("C# Programming",4),
                    new Course("Java Programming",5),
                    new Course("Artifical Intelligence",5),
                    new Course("Machine Learning",5)
             };

                _context.Courses.AddRange(courseList);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EnrollStudentAsync(int studentId, int courseId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var isStudentIdExist = await _context.Students.AnyAsync(x => x.StudentId == studentId);
                var isCourseIdExist = await _context.Courses.AnyAsync(x => x.CourseId == courseId);

                if (!isStudentIdExist || !isCourseIdExist)
                {
                    throw new Exception("Student or course cannot be found!");
                }

                var record = await _context.StudentCourses.AnyAsync(x => (x.StudentId == studentId && x.CourseId == courseId));

                if (record)
                {
                    throw new Exception("Student already added this course!");
                }
                var studentCourse = new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = courseId
                };
                await _context.StudentCourses.AddAsync(studentCourse);
                await _context.SaveChangesAsync();


            }

        }

        public async Task<int> GetCourseCountAsync()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Courses.CountAsync();
            }
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Courses.ToListAsync();
            }
        }

        public async Task<List<StudentCourse>> GetEnrollmentsAsync()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                
                return await _context.StudentCourses.
                    Include(x => x.Student)
                    .Include(x => x.Course)
                    .ToListAsync();
                
            }
        }

        public async Task AddEnrollmentsAsync()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                await _context.StudentCourses.ToListAsync();
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetEnrollmentCount()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.StudentCourses.CountAsync();
                
            }

        }

        public async Task DeleteEnrollment(int studentId, int courseId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var record = _context.StudentCourses.FirstOrDefault(x => (x.StudentId == studentId && x.CourseId == courseId));
                if (record != null)
                {
                    _context.StudentCourses.Remove(record);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResult<StudentCourse>> FilteringProcesses(string searchText, int? courseId, int pageNumber, int pageSize)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var tempList = _context.StudentCourses
                    .Include(x => x.Student)
                    .Include(x => x.Course)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    tempList = tempList.Where(x => (x.Student.Name.Contains(searchText) || x.Student.LastName.Contains(searchText) || x.Course.CourseName.Contains(searchText)));
                }
                if (courseId>0)
                {
                    tempList = tempList.Where(x => (x.CourseId == courseId));
                }               

                int totalEnrollments = await tempList.CountAsync();

                int totalPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalEnrollments / pageSize));

                var courses = await tempList.AsNoTracking()
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize).ToListAsync();

                return new PaginatedResult<StudentCourse>(courses, totalEnrollments, totalPageNumber, pageNumber, pageSize);
            }
        }

        public async Task<List<StudentCourse>> SearchStudents(string searchText)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var searchedList = _context.StudentCourses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText)){
                    searchedList = _context.StudentCourses.Where(x => (x.Student.Name.Contains(searchText) || x.Student.LastName.Contains(searchText)|| x.Course.CourseName.Contains(searchText)));
                }              
                return await searchedList.
                    Include(x => x.Student)
                    .Include(x => x.Course)
                     .ToListAsync();

            }
        }

        public async Task<List<StudentCourse>> GetSelectedCourse(int courseId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var selectedList = _context.StudentCourses.AsQueryable();

                selectedList = _context.StudentCourses.Where(x => (x.CourseId == courseId));

                return await selectedList.
                    Include(x => x.Student)
                    .Include(x => x.Course)
                    .ToListAsync();

            }
        }
    }
}