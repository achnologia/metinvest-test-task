using Microsoft.AspNetCore.Mvc;

namespace Metinvest.API.Controllers;

[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHelloWorld()
    {
        return Ok(await Task.FromResult("Hello World!"));
    }
}