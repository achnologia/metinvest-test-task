using Metinvest.Application.Shared;
using Metinvest.Domain.Entities;
using Metinvest.Domain.Exceptions;
using Metinvest.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Application.StudentCourses.Services;

public class StudentCourseService : IStudentCourseService
{
    private readonly ApplicationDbContext _context;

    public StudentCourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AssociateCourseWithStudentAsync(string fullName, string email, int idCourse, DateTime startDate, DateTime endDate)
    {
        var student = await _context.Students.Include(x => x.Courses).SingleOrDefaultAsync(x => x.Email == email);

        if (student is null)
            return false;

        var course = await _context.Courses.SingleOrDefaultAsync(x => x.Id == idCourse);

        if (course is null)
            return false;

        if(startDate > endDate)
            throw new UserFriendlyException("Start date cannot be after End date");
        
        if (startDate.DayOfWeek != DayOfWeek.Monday)
            throw new UserFriendlyException("Start date should be on Monday");

        if (endDate.DayOfWeek != DayOfWeek.Friday)
            throw new UserFriendlyException("End date should be on Friday");

        if (student.HasOverlappingCourse(startDate, endDate))
            throw new UserFriendlyException("There's an overlapping course for this period");

        var studentCourse = new StudentCourse(student.Id, idCourse, startDate, endDate);
        
        await _context.AddAsync(studentCourse);
        await _context.SaveChangesAsync();

        return true;
    }
}