using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Services.Interface;

public interface IEventService
{

    //CREATE 
    Task<EventDTO?> AddEventAsync(EventDTO eventDTO);


    //Read
    Task<IEnumerable<EventDTO>> GetAllEventsAsync(int page, int pageSize);
    Task<EventDTO?> GetEventByIdAsync(int id);

    // UPDATE
    Task<EventDTO?> UpdateEventAsync(int id, EventDTO eventDTO, int loginId);


    // DELETE
    Task<EventDTO?> DeleteEventAsync(int id, int loginId);

    
}
