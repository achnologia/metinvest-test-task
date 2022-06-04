using Metinvest.API.Contracts.Shared;

namespace Metinvest.API.Contracts.Responses.Students;

public record GetAllStudentsResponse(IEnumerable<StudentMinimalDto> Students);