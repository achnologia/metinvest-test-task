using Metinvest.Domain.Entities;

namespace Metinvest.Application.Students.Services;

public interface IStudentService
{
    public Task<IEnumerable<Student>> GetAllAsync();
    public Task<int> CreateAsync(string firstName, string lastName, string email);
    public Task<Student> GetByIdAsync(int id);
    public Task<bool> DeleteAsync(int id);
}