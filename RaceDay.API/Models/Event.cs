using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Event
{
    public int EventId { get; set; }
    [Required, MaxLength(120)] public string Name { get; set; } = string.Empty;
    [MaxLength(1000)] public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    [Required, MaxLength(200)] public string Location { get; set; } = string.Empty;
    [Range(0.1, 1000)] public decimal Distance { get; set; }
    [Required, RegularExpression("^(run|walk|cycle)$", ErrorMessage = "EventType must be run, walk, or cycle.")] public string EventType { get; set; } = "run";
    public int OrganiserId { get; set; }
    public Organiser? Organiser { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
}
