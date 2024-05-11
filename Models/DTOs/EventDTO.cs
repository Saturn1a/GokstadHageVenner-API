namespace GokstadHageVennerAPI.Models.DTOs;

public class EventDTO
{
    public EventDTO(int id, int memberId, string eventName, string eventDescription, DateTime eventDate)
    {
        Id = id;
        MemberId = memberId;
        EventName = eventName;
        EventDescription = eventDescription;
        EventDate = new DateTime(
            eventDate.Year,
            eventDate.Month,
            eventDate.Day,
            eventDate.Hour,
            eventDate.Minute,
            0);
      
    }

    public int Id { get; set; }

    public int MemberId { get; set; }

    public string EventName { get; set; } = string.Empty;

    public string EventDescription { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }



}
