using GokstadHageVennerAPI.Data;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GokstadHageVennerAPI.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly GokstadHageVennerDbContext _dbContext;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(GokstadHageVennerDbContext dbContext, ILogger<EventRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<Event?> AddEventAsync(Event gardenEvent)
        {
            _logger.LogDebug("Adding event to database");

            var entity = await _dbContext.Events.AddAsync(gardenEvent);

            await _dbContext.SaveChangesAsync();
            if (entity == null) return null;

            return entity.Entity;
              
           
            
        }

        public  async Task<Event?> GetEventByIdAsync(int id)
        {
            _logger.LogDebug("Getting event with id:{@id} from database", id);

            var evt = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (evt == null) return null;
            return evt;

        }
        

        public async Task<ICollection<Event>> GetEventsAsync(int page, int pageSize)
        {
            _logger.LogDebug("Getting all events from database");

            var totCount = _dbContext.Events.Count();
            if (totCount == 0)
            {
                return Enumerable.Empty<Event>().ToList();
            }

            return await _dbContext.Events
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<Event?> DeleteEventByIdAsync(int id)
        {
            _logger.LogDebug("Deleting event with id:{@id} from database", id);

            var evt = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (evt == null) return null;

            var entity = _dbContext.Events.Remove(evt);
            await _dbContext.SaveChangesAsync();

            if (entity == null) return null;
            return entity.Entity;

        }

        public async Task<Event?> UpdateEventAsync(int id, Event gardenEvent)
        {
            _logger.LogDebug("Updating event with id:{@id} in database", id);

            var affectedRows = await _dbContext.Events.Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.EventName, gardenEvent.EventName)
                .SetProperty(x => x.EventDescription, gardenEvent.EventDescription)
                .SetProperty(x => x.EventDate, gardenEvent.EventDate));


            if (affectedRows == 0) return null;
            return gardenEvent;

          
        }
    }
}

