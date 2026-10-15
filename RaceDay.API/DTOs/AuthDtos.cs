using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.DTOs;

public class RegisterRequest
{
    [Required, MaxLength(60)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(60)] public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
    [Required, RegularExpression("^(Organiser|Participant)$", ErrorMessage = "Role must be Organiser or Participant.")] public string Role { get; set; } = "Participant";
}

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class EventRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    [Required] public string Location { get; set; } = string.Empty;
    [Range(0.1, 1000)] public decimal Distance { get; set; }
    [RegularExpression("^(run|walk|cycle)$")] public string EventType { get; set; } = "run";
}

public class CategoryRequest
{
    [Required] public string CategoryName { get; set; } = string.Empty;
    [Required] public string CategoryType { get; set; } = "Age";
}

public class EnrolRequest
{
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
}

public class ResultRequest
{
    public TimeSpan FinishTime { get; set; }
    [Range(1, int.MaxValue)] public int FinishingPosition { get; set; }
}
