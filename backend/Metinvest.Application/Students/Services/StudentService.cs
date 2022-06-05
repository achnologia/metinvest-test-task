using Metinvest.Application.Shared;
using Metinvest.Domain.Entities;
using Metinvest.Domain.Events;
using Metinvest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Application.Students.Services;

public class StudentService : IStudentService
{
    private readonly IApplicationDbContext _context;

    public StudentService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken token)
    {
        return await _context.Students.ToListAsync(token);
    }

    public async Task<int> CreateAsync(string firstName, string lastName, string email, CancellationToken token)
    {
        var existingStudent = await _context.Students.SingleOrDefaultAsync(x => x.Email == email, token);

        if (existingStudent is not null)
            throw new UserFriendlyException("The student with such email already exists");

        var newStudent = new Student(firstName, lastName, email);

        await _context.Students.AddAsync(newStudent, token);
        await _context.SaveChangesAsync(token);

        return newStudent.Id;
    }

    public async Task<Student?> GetByIdAsync(int id, CancellationToken token)
    {
        return await _context.Students
            .Include(x => x.Courses).ThenInclude(x => x.Course)
            .Include(x => x.Holidays)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token)
    {
        var student = await GetByIdAsync(id, token);

        if (student is null)
            return false;

        _context.Students.Remove(student);
        var deleted = await _context.SaveChangesAsync(token);

        return deleted > 0;
    }

    public async Task<bool> CreateHolidayAsync(int id, DateTime startDate, DateTime endDate, CancellationToken token)
    {
        var student = await GetByIdAsync(id, token);

        if (student is null)
            return false;
        
        if(startDate > endDate)
            throw new UserFriendlyException("Start date cannot be after End date");
        
        if (startDate.DayOfWeek != DayOfWeek.Monday)
            throw new UserFriendlyException("Start date should be on Monday");

        if (endDate.DayOfWeek != DayOfWeek.Friday)
            throw new UserFriendlyException("End date should be on Friday");

        if (student.HasOverlappingHoliday(startDate, endDate))
            throw new UserFriendlyException("There's an overlapping holiday for this period");

        var holiday = new Holiday(student.Id, startDate, endDate);
        student.AddHoliday(holiday);
        
        var e = new HolidayCreatedEvent(student, holiday);
        student.AddDomainEvent(e);
        
        _context.Students.Update(student);
        await _context.SaveChangesAsync(token);

        return true;
    }
}