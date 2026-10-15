using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class User : IdentityUser<int>
{
    [Required, MaxLength(60)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(60)] public string LastName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public Organiser? Organiser { get; set; }
    public Participant? Participant { get; set; }
}
