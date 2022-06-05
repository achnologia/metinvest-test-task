using Metinvest.Domain.Entities;

namespace Metinvest.Application.Courses.Services;

public interface ICourseService
{
    public Task<IEnumerable<Course>> GetAllAsync(CancellationToken token);
    public Task<int> CreateAsync(string courseName, CancellationToken token);
    public Task<Course?> GetByIdAsync(int id, CancellationToken token);
    public Task<bool> DeleteAsync(int id, CancellationToken token);
    public Task ExtendCourseDuration(Student student, Holiday holiday, CancellationToken token);
}