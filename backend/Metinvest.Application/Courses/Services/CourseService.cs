using Metinvest.Application.Shared;
using Metinvest.Domain.Entities;
using Metinvest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Application.Courses.Services;

public class CourseService : ICourseService
{
    private readonly IApplicationDbContext _context;

    public CourseService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken token)
    {
        return await _context.Courses.ToListAsync(token);
    }

    public async Task<int> CreateAsync(string courseName, CancellationToken token)
    {
        var existingCourse = await _context.Courses.SingleOrDefaultAsync(x => x.CourseName == courseName, token);

        if (existingCourse is not null)
            throw new UserFriendlyException("The course with such name already exists");

        var newCourse = new Course(courseName);

        await _context.Courses.AddAsync(newCourse, token);
        await _context.SaveChangesAsync(token);

        return newCourse.Id;
    }

    public async Task<Course?> GetByIdAsync(int id, CancellationToken token)
    {
        return await _context.Courses.SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token)
    {
        var course = await GetByIdAsync(id, token);

        if (course is null)
            return false;

        _context.Courses.Remove(course);
        var deleted = await _context.SaveChangesAsync(token);

        return deleted > 0;
    }

    public async Task ExtendCourseDuration(Student student, Holiday holiday, CancellationToken token)
    {
        var course = student.GetCourseOnDate(holiday.StartDate);

        if (course is null)
            return;

        var newEndDate = course.EndDate.AddDays(holiday.TotalWeeks * 7);
        
        if (student.HasOverlappingCourse(course.IdCourse, newEndDate))
            throw new UserFriendlyException("There's an overlapping course for this period");
        
        course.UpdateEndDate(newEndDate);

        _context.Update(course);
        await _context.SaveChangesAsync(token);
    }
}