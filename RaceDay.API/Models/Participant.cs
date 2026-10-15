namespace RaceDay.API.Models;

public class Participant
{
    public int ParticipantId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
}
