using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Repository.Interface;

public interface IEventRepository
{
    // CREATE
    Task<Event?> AddEventAsync(Event gardenEvent);


    // READ
    Task<ICollection<Event>> GetEventsAsync(int page, int pageSize);
    Task<Event?> GetEventByIdAsync(int id);


    // UPDATE
    Task<Event?> UpdateEventAsync(int id, Event gardenEvent);



    // DELETE
    Task<Event?> DeleteEventByIdAsync(int id);
}
