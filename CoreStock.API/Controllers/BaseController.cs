using Microsoft.AspNetCore.Mvc;

namespace CoreStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleException(Exception ex) => ex switch
    {
        KeyNotFoundException => NotFound(new { error = ex.Message }),
        InvalidOperationException => BadRequest(new { error = ex.Message }),
        ArgumentException => BadRequest(new { error = ex.Message }),
        _ => StatusCode(500, new { error = "An unexpected error occurred." })
    };
}
