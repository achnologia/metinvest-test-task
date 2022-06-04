using Metinvest.Domain.Entities;
using Metinvest.Domain.Exceptions;
using Metinvest.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Application.Students.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<int> CreateAsync(string firstName, string lastName, string email)
    {
        var existingStudent = await _context.Students.SingleOrDefaultAsync(x => x.Email == email);

        if (existingStudent is not null)
            throw new UserFriendlyException("The student with such email already exists");

        var newStudent = new Student(firstName, lastName, email);

        await _context.Students.AddAsync(newStudent);
        await _context.SaveChangesAsync();

        return newStudent.Id;
    }

    public async Task<Student> GetByIdAsync(int id)
    {
        return await _context.Students.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await GetByIdAsync(id);

        if (student is null)
            return false;

        _context.Students.Remove(student);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}