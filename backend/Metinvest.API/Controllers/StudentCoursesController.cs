using Metinvest.API.Contracts.Requests.StudentCourses;
using Metinvest.Application.StudentCourses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Metinvest.API.Controllers;

[Route("api/[controller]")]
public class StudentCoursesController : ControllerBase
{
    private readonly IStudentCourseService _service;

    public StudentCoursesController(IStudentCourseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AssociateCourseWithStudent([FromBody] AssociateCourseWithStudentRequest request)
    {
        var success = await _service.AssociateCourseWithStudentAsync(request.FullName, request.Email, request.IdCourse, request.StartDate, request.EndDate);

        if (!success)
            return NotFound();

        return Ok();
    }
}