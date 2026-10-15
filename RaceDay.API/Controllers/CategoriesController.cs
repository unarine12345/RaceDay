using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/categories")]
public class CategoriesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Get(int eventId) => Ok(await db.Categories.Where(c => c.EventId == eventId).OrderBy(c => c.CategoryName).ToListAsync());

    [HttpPost, Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Create(int eventId, CategoryRequest request)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var item = await db.Events.SingleOrDefaultAsync(e => e.EventId == eventId && e.Organiser!.UserId == uid);
        if (item is null) return NotFound(new { message = "Event not found or not owned by this organiser." });
        var category = new Category { EventId = eventId, CategoryName = request.CategoryName.Trim(), CategoryType = request.CategoryType.Trim() };
        db.Categories.Add(category); await db.SaveChangesAsync(); return Created($"/api/events/{eventId}/categories", category);
    }

    [HttpDelete("{categoryId:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Delete(int eventId, int categoryId)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var category = await db.Categories.Include(c => c.Event).ThenInclude(e => e!.Organiser).SingleOrDefaultAsync(c => c.EventId == eventId && c.CategoryId == categoryId);
        if (category is null) return NotFound(); if (category.Event!.Organiser!.UserId != uid) return Forbid();
        db.Categories.Remove(category); await db.SaveChangesAsync(); return NoContent();
    }
}
