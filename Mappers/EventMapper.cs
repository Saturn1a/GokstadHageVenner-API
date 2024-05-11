using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Mappers;

public class EventMapper : Imapper<Event, EventDTO>
{
    public EventDTO MapToDTO(Event model)
    {
        return new EventDTO(model.Id, model.MemberId, model.EventName, model.EventDescription, model.EventDate);
    }

    public Event MapToModel(EventDTO dto)
    {
        return new Event()
        {
            Id = dto.Id,
            MemberId = dto.MemberId,
            EventName = dto.EventName,
            EventDescription = dto.EventDescription,
            EventDate = dto.EventDate

        };
    }
}
