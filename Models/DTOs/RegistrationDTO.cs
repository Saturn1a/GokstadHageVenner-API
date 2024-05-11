namespace GokstadHageVennerAPI.Models.DTOs;

public class RegistrationDTO
{
    public RegistrationDTO(int id, int eventId, int memberId, string message)
    {
        Id = id;
        EventId = eventId;
        MemberId = memberId;
        Message = message;
    }

    public int Id { get; set; }

    public int EventId { get; set; }

    public int MemberId { get; set; }

    public string Message { get; set; } = string.Empty;




}
