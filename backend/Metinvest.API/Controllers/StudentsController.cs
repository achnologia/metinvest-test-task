using Metinvest.API.Contracts.Requests.Students;
using Metinvest.API.Contracts.Responses.Students;
using Metinvest.API.Contracts.Shared;
using Metinvest.Application.Students.Services;
using Microsoft.AspNetCore.Mvc;

namespace Metinvest.API.Controllers;

[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStudents(CancellationToken token)
    {
        var students = await _service.GetAllAsync(token);

        var response = new GetAllStudentsResponse(students.Select(x => new StudentMinimalDto(x.Id, x.FullName, x.Email)));
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request, CancellationToken token)
    {
        var idCreated = await _service.CreateAsync(request.FirstName, request.LastName, request.Email, token);

        return CreatedAtRoute("StudentLink", new { id = idCreated }, request);
    }
    
    [HttpGet("{id}", Name = "StudentLink")]
    public async Task<IActionResult> GetStudent([FromRoute]int id, CancellationToken token)
    {
        var student = await _service.GetByIdAsync(id, token);

        if(student is null)
            return NotFound();

        var courses = Enumerable.Empty<CourseDto>();
        var holidays = Enumerable.Empty<HolidayDto>();

        if (student.Courses is not null)
        {
            courses = student.Courses.Select(x => new CourseDto(x.Course.CourseName, x.StartDate, x.EndDate));
        }
        
        if (student.Holidays is not null)
        {
            holidays = student.Holidays.Select(x => new HolidayDto(x.StartDate, x.EndDate));
        }
        
        var response = new GetStudentResponse(student.Id, student.FullName, student.Email, courses, holidays);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] int id, CancellationToken token)
    {
        var deleted = await _service.DeleteAsync(id, token);

        if(!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/holidays")]
    public async Task<IActionResult> CreateHoliday([FromRoute]int id, [FromBody] CreateHolidayRequest request, CancellationToken token)
    {
        var success = await _service.CreateHolidayAsync(id, request.StartDate, request.EndDate, token);

        if (!success)
            return NotFound();

        return NoContent();
    }
}