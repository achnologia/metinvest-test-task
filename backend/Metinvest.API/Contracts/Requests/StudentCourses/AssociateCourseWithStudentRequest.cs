namespace Metinvest.API.Contracts.Requests.StudentCourses;

public record AssociateCourseWithStudentRequest(string FullName, string Email, int IdCourse, DateTime StartDate, DateTime EndDate);