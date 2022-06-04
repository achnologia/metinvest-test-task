using Metinvest.Domain.Entities;
using Metinvest.Domain.Exceptions;
using Metinvest.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Application.Courses.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<int> CreateAsync(string courseName)
    {
        var existingCourse = await _context.Courses.SingleOrDefaultAsync(x => x.CourseName == courseName);

        if (existingCourse is not null)
            throw new UserFriendlyException("The course with such name already exists");

        var newCourse = new Course(courseName);

        await _context.Courses.AddAsync(newCourse);
        await _context.SaveChangesAsync();

        return newCourse.Id;
    }

    public async Task<Course> GetByIdAsync(int id)
    {
        return await _context.Courses.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await GetByIdAsync(id);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}