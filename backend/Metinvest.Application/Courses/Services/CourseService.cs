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
        ManageExtendCourseByStartDate(student, holiday.StartDate, holiday.TotalWeeks);
        ManageExtendCourseByEndDate(student, holiday.EndDate, holiday.TotalWeeks);
        
        await _context.SaveChangesAsync(token);
    }

    private void ManageExtendCourseByStartDate(Student student, DateTime startDate, int holidayNumberOfWeeks)
    {
        var course = student.GetCourseOnDate(startDate);

        if (course is null)
            return;

        var numberOfWeeks = course.TotalWeeks >= holidayNumberOfWeeks ? holidayNumberOfWeeks : course.TotalWeeks;
        
        var newEndDate = course.EndDate.AddDays(numberOfWeeks * 7);
        
        if (student.HasOverlappingCourse(course.IdCourse, newEndDate))
            throw new UserFriendlyException("There's an overlapping course for this period");
        
        course.UpdateEndDate(newEndDate);
        
        _context.Update(course);
    }
    
    private void ManageExtendCourseByEndDate(Student student, DateTime endDate, int holidayNumberOfWeeks)
    {
        var course = student.GetCourseOnDate(endDate);

        if (course is null)
            return;

        var numberOfWeeks = course.TotalWeeks >= holidayNumberOfWeeks ? holidayNumberOfWeeks : course.TotalWeeks;
        
        var newEndDate = course.EndDate.AddDays(numberOfWeeks * 7);
        
        if (student.HasOverlappingCourse(course.IdCourse, newEndDate))
            throw new UserFriendlyException("There's an overlapping course for this period");
        
        course.UpdateEndDate(newEndDate);
        
        _context.Update(course);
    }
}