using Metinvest.API.Contracts.Shared;

namespace Metinvest.API.Contracts.Responses.Students;

public record GetStudentResponse(int Id, string FullName, string Email, IEnumerable<CourseDto> Courses, IEnumerable<HolidayDto> Holidays);