using Metinvest.Domain.Entities;

namespace Metinvest.Application.Students.Services;

public interface IStudentService
{
    public Task<IEnumerable<Student>> GetAllAsync(CancellationToken token);
    public Task<int> CreateAsync(string firstName, string lastName, string email, CancellationToken token);
    public Task<Student?> GetByIdAsync(int id, CancellationToken token);
    public Task<bool> DeleteAsync(int id, CancellationToken token);
    public Task<bool> CreateHolidayAsync(int id, DateTime startDate, DateTime endDate, CancellationToken token);
}