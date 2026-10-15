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
[Authorize(Roles = "Participant")]
public class EnrolmentsController(ApplicationDbContext db) : ControllerBase
{
    [HttpPost("events/{eventId:int}")]
    public async Task<IActionResult> Enrol(int eventId, EnrolRequest request)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var participant = await db.Participants.SingleOrDefaultAsync(p => p.UserId == uid);
        if (participant is null) return BadRequest(new { message = "Participant profile is missing." });
        if (!await db.Events.AnyAsync(e => e.EventId == eventId)) return NotFound(new { message = "Event not found." });
        if (!await db.Categories.AnyAsync(c => c.CategoryId == request.CategoryId && c.EventId == eventId)) return BadRequest(new { message = "The category does not belong to this event." });
        if (await db.EventEnrolments.AnyAsync(e => e.EventId == eventId && e.ParticipantId == participant.ParticipantId)) return Conflict(new { message = "You are already enrolled in this event." });
        var enrolment = new EventEnrolment { EventId = eventId, ParticipantId = participant.ParticipantId, CategoryId = request.CategoryId };
        db.EventEnrolments.Add(enrolment); await db.SaveChangesAsync(); return Created($"/api/enrolments/{enrolment.EventEnrolmentId}", enrolment);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> Mine()
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await db.EventEnrolments.Where(e => e.Participant!.UserId == uid).Include(e => e.Event).Include(e => e.Category).Include(e => e.Result).Select(e => new { e.EventEnrolmentId, Event = e.Event!.Name, e.Event.EventDate, Category = e.Category!.CategoryName, e.EnrolmentDate, Result = e.Result == null ? null : new { e.Result.FinishTime, e.Result.FinishingPosition } }).ToListAsync());
    }
}
