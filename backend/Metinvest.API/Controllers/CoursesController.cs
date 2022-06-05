using Metinvest.API.Contracts.Requests.Courses;
using Metinvest.API.Contracts.Responses.Courses;
using Metinvest.Application.Courses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Metinvest.API.Controllers;

[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableCourses(CancellationToken token)
    {
        var courses = await _service.GetAllAsync(token);

        var response = new GetAllCoursesResponse(courses.Select(x => x.CourseName));
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken token)
    {
        var idCreated = await _service.CreateAsync(request.CourseName, token);

        return CreatedAtRoute("CourseLink", new { id = idCreated }, request);
    }
    
    [HttpGet("{id}", Name = "CourseLink")]
    public async Task<IActionResult> GetCourse([FromRoute]int id, CancellationToken token)
    {
        var course = await _service.GetByIdAsync(id, token);

        if(course is null)
            return NotFound();

        var response = new GetCourseResponse(course.CourseName);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse([FromRoute] int id, CancellationToken token)
    {
        var deleted = await _service.DeleteAsync(id, token);

        if(!deleted)
            return NotFound();

        return NoContent();
    }
}