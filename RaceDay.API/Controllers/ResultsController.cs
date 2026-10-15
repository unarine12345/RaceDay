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
public class ResultsController(ApplicationDbContext db) : ControllerBase
{
    [HttpPost("enrolments/{enrolmentId:int}"), Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Record(int enrolmentId, ResultRequest request)
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var enrolment = await db.EventEnrolments.Include(e => e.Event).ThenInclude(e => e!.Organiser).SingleOrDefaultAsync(e => e.EventEnrolmentId == enrolmentId);
        if (enrolment is null) return NotFound();
        if (enrolment.Event!.Organiser!.UserId != uid) return Forbid();
        if (request.FinishTime < TimeSpan.Zero) return BadRequest(new { message = "Finish time cannot be negative." });
        var result = await db.Results.SingleOrDefaultAsync(r => r.EventEnrolmentId == enrolmentId);
        if (result is null) { result = new Result { EventEnrolmentId = enrolmentId }; db.Results.Add(result); }
        result.FinishTime = request.FinishTime; result.FinishingPosition = request.FinishingPosition;
        await db.SaveChangesAsync(); return Ok(result);
    }

    [HttpGet("mine"), Authorize(Roles = "Participant")]
    public async Task<IActionResult> Mine()
    {
        var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await db.Results.Where(r => r.EventEnrolment!.Participant!.UserId == uid).Include(r => r.EventEnrolment).ThenInclude(e => e!.Event).Select(r => new { Event = r.EventEnrolment!.Event!.Name, r.FinishTime, r.FinishingPosition }).ToListAsync());
    }
}
