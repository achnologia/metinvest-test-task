namespace Metinvest.Application.StudentCourses.Services;

public interface IStudentCourseService
{
    Task<bool> AssociateCourseWithStudentAsync(string fullName, string email, int idCourse, DateTime startDate, DateTime endDate, CancellationToken token);
}