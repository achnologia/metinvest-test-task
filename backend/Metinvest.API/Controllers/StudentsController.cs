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
    public async Task<IActionResult> GetStudents()
    {
        var students = await _service.GetAllAsync();

        var response = new GetAllStudentsResponse(students.Select(x => new StudentMinimalDto(x.Id, x.FullName, x.Email)));
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var idCreated = await _service.CreateAsync(request.FirstName, request.LastName, request.Email);

        return CreatedAtRoute("StudentLink", new { id = idCreated }, request);
    }
    
    [HttpGet("{id}", Name = "StudentLink")]
    public async Task<IActionResult> GetStudent([FromRoute]int id)
    {
        var student = await _service.GetByIdAsync(id);

        if(student is null)
            return NotFound();

        var courses = Enumerable.Empty<string>();

        if (student.Courses is not null)
        {
            courses = student.Courses?.Select(x => x.Course).Select(x => x.CourseName);
        }
        
        var response = new GetStudentResponse(student.Id, student.FullName, student.Email, courses);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if(!deleted)
            return NotFound();

        return NoContent();
    }
}