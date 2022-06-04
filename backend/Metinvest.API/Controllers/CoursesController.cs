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
    public async Task<IActionResult> GetAvailableCourses()
    {
        var courses = await _service.GetAllAsync();

        var response = new GetAllCoursesResponse(courses.Select(x => x.CourseName));
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        var idCreated = await _service.CreateAsync(request.CourseName);

        return CreatedAtRoute("CourseLink", new { id = idCreated }, request);
    }
    
    [HttpGet("{id}", Name = "CourseLink")]
    public async Task<IActionResult> GetCourse([FromRoute]int id)
    {
        var course = await _service.GetByIdAsync(id);

        if(course is null)
            return NotFound();

        var response = new GetCourseResponse(course.CourseName);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse([FromRoute] int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if(!deleted)
            return NotFound();

        return NoContent();
    }
}