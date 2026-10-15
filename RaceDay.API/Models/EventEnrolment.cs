namespace RaceDay.API.Models;

public class EventEnrolment
{
    public int EventEnrolmentId { get; set; }
    public int ParticipantId { get; set; }
    public Participant? Participant { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;
    public Result? Result { get; set; }
}
