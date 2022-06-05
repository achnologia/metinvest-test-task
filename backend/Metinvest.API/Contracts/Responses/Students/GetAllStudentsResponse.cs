using Metinvest.API.Contracts.Common;

namespace Metinvest.API.Contracts.Responses.Students;

public record GetAllStudentsResponse(IEnumerable<StudentMinimalDto> Students);