namespace RaceDay.API.Models;

public class Organiser
{
    public int OrganiserId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
