using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers;

public record UpdateProfileRequest(string FirstName, string LastName);

[ApiController, Route("api/profile"), Authorize]
public class ProfileController(ApplicationDbContext db) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProfileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName)) return BadRequest(new { message = "First name and last name are required." });
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null) return NotFound();
        user.FirstName = request.FirstName.Trim(); user.LastName = request.LastName.Trim(); await db.SaveChangesAsync();
        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email });
    }
}
