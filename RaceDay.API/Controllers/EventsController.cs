using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvents() => Ok(await db.Events.Include(e => e.Categories).OrderBy(e => e.EventDate).ToListAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvent(int id) { var item = await db.Events.Include(e => e.Categories).SingleOrDefaultAsync(e => e.EventId == id); return item is null ? NotFound() : Ok(item); }

    [HttpPost, Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateEvent(EventRequest request)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var organiser = await db.Organisers.SingleOrDefaultAsync(o => o.UserId == uid);
        if (organiser is null) return BadRequest(new { message = "Organiser profile is missing." });
        var item = new Event { Name = request.Name, Description = request.Description, EventDate = request.EventDate, Location = request.Location, Distance = request.Distance, EventType = request.EventType, OrganiserId = organiser.OrganiserId };
        db.Events.Add(item); await db.SaveChangesAsync(); return CreatedAtAction(nameof(GetEvent), new { id = item.EventId }, item);
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(int id, EventRequest request)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var item = await db.Events.Include(e => e.Organiser).SingleOrDefaultAsync(e => e.EventId == id);
        if (item is null) return NotFound(); if (item.Organiser?.UserId != uid) return Forbid();
        item.Name = request.Name; item.Description = request.Description; item.EventDate = request.EventDate; item.Location = request.Location; item.Distance = request.Distance; item.EventType = request.EventType;
        await db.SaveChangesAsync(); return Ok(item);
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var item = await db.Events.Include(e => e.Organiser).SingleOrDefaultAsync(e => e.EventId == id);
        if (item is null) return NotFound(); if (item.Organiser?.UserId != uid) return Forbid();
        db.Events.Remove(item); await db.SaveChangesAsync(); return NoContent();
    }

    [HttpGet("{id:int}/enrolments"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> GetEnrolments(int id)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var owned = await db.Events.AnyAsync(e => e.EventId == id && e.Organiser!.UserId == uid);
        if (!owned) return NotFound();
        var rows = await db.EventEnrolments.Where(x => x.EventId == id).Include(x => x.Participant)!.ThenInclude(p => p!.User).Include(x => x.Category).Include(x => x.Result).ToListAsync();
        return Ok(rows.Select(x => new { x.EventEnrolmentId, Participant = x.Participant!.User!.FirstName + " " + x.Participant.User.LastName, x.Category!.CategoryName, x.EnrolmentDate, Result = x.Result == null ? null : new { x.Result.FinishTime, x.Result.FinishingPosition } }));
    }
}
