using Microsoft.AspNetCore.Mvc;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>Checks whether the RaceDay API is responding.</summary>
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy", service = "RaceDay.API" });
}
