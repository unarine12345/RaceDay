using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Category
{
    public int CategoryId { get; set; }
    [Required, MaxLength(100)] public string CategoryName { get; set; } = string.Empty;
    [Required, MaxLength(30)] public string CategoryType { get; set; } = "Age";
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
}
