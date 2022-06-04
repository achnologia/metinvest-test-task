using Metinvest.Domain.Entities;

namespace Metinvest.Application.Courses.Services;

public interface ICourseService
{
    public Task<IEnumerable<Course>> GetAllAsync();
    public Task<int> CreateAsync(string courseName);
    public Task<Course> GetByIdAsync(int id);
    public Task<bool> DeleteAsync(int id);
}