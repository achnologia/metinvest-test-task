namespace Metinvest.API.Contracts.Responses.Students;

public record GetStudentResponse(int Id, string FullName, string Email, IEnumerable<string> Courses);