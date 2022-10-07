using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Helpers;

namespace SchoolOfDevs.Services
{
    public interface ICourseService
    {
        public Task<Course> Create(Course Course);
        public Task<Course> GetById(int Id);
        public Task<List<Course>> GetAll();
        public Task Update(Course courseIn, int id);
        public Task Delete(int id);
    }
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }
        public async Task<Course> Create(Course course)
        {
            Course CourseDb = await _context.Courses
                .AsNoTracking() //Exibe uma excessão caso tentem trackear mais de 1 elemento no banco
                .SingleOrDefaultAsync(u => u.Name == course.Name);
            if (CourseDb is not null)
            {
                throw new Exception($"CourseName {course.Name} already exist.");
            }
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task Delete(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);
            if (courseDb is null)
            {
                throw new Exception($"Course{id} not found");
            }
            _context.Courses.Remove(courseDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAll() => await _context.Courses.ToListAsync();

        public async Task<Course> GetById(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);
            if (courseDb is null)
            {
                throw new Exception($"Course{id} not found");
            }
            return courseDb;
        }

        public async Task Update(Course courseIn, int id)
        {
            if (courseIn.Id != id)
            {
                throw new Exception("Route id differs Course id");
            }
            Course courseDb = await _context.Courses
                .AsNoTracking()
               .SingleOrDefaultAsync(u => u.Id == id);
            if (courseDb is null)
            {
                throw new Exception($"Course{id} not found");
            }
            _context.Entry(courseIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
