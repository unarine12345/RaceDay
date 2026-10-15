using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDbContext db, IPasswordHasher<User> hasher) : ControllerBase
{
    /// <summary>Registers an Organiser or Participant account. Passwords are stored as hashes.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant())) return Conflict(new { message = "An account with this email already exists." });
        var roleName = request.Role.Trim();
        var role = await db.RaceDayRoles.SingleOrDefaultAsync(r => r.RoleName == roleName);
        if (role is null) { role = new Role { RoleName = roleName }; db.RaceDayRoles.Add(role); await db.SaveChangesAsync(); }
        var user = new User { UserName = email, Email = email, NormalizedEmail = email.ToUpperInvariant(), NormalizedUserName = email.ToUpperInvariant(), FirstName = request.FirstName.Trim(), LastName = request.LastName.Trim(), RoleId = role.RoleId, EmailConfirmed = true };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        if (roleName == "Organiser") db.Organisers.Add(new Organiser { UserId = user.Id });
        else db.Participants.Add(new Participant { UserId = user.Id });
        await db.SaveChangesAsync();
        return Created("/api/auth/me", new { user.Id, user.FirstName, user.LastName, user.Email, Role = roleName });
    }

    /// <summary>Signs in and issues an HTTP-only authentication cookie.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToUpperInvariant();
        var user = await db.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.NormalizedEmail == email);
        if (user is null || hasher.VerifyHashedPassword(user, user.PasswordHash ?? "", request.Password) == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid email or password." });
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, user.Email ?? ""), new(ClaimTypes.Role, user.Role?.RoleName ?? "Participant") };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return Ok(new { message = "Login successful.", user.Id, user.FirstName, user.LastName, user.Email, Role = user.Role?.RoleName });
    }

    [HttpPost("logout"), Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); return Ok(new { message = "Logged out." }); }

    [HttpGet("me"), Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Me()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)) return Unauthorized();
        var user = await db.Users.Include(u => u.Role).Where(u => u.Id == id).Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, Role = u.Role!.RoleName }).SingleOrDefaultAsync();
        return user is null ? NotFound() : Ok(user);
    }
}
