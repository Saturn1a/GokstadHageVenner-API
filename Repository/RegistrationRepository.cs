using GokstadHageVennerAPI.Data;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GokstadHageVennerAPI.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly GokstadHageVennerDbContext _dbContext;
    private readonly ILogger<RegistrationRepository> _logger;

    public RegistrationRepository(GokstadHageVennerDbContext dbContext, ILogger<RegistrationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Registration?> AddRegistrationAsync(Registration registration)
    {
        _logger.LogDebug("Adding registration to database");

        var AddedReg = await _dbContext.Registration.AddAsync(registration);
        await _dbContext.SaveChangesAsync();

        
        if (AddedReg == null) return null;
        return AddedReg.Entity;
        

    }

    public async Task<Registration?> GetRegistrationByIdAsync(int id)
    {
        _logger.LogDebug("Getting registration with id:{@id} from database", id);
        var reg = await _dbContext.Registration.FirstOrDefaultAsync(x => x.Id == id);

        if (reg == null) return null;
        return reg;

    }

    public async Task<ICollection<Registration>> GetRegistrationsAsync(int page, int pageSize)
    {
        _logger.LogDebug("Getting all registrations from database");
        var totCount = _dbContext.Registration.Count();
        if (totCount == 0)
        {
            return Enumerable.Empty<Registration>().ToList();
        }

        return await _dbContext.Registration
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    }

    public async Task<Registration?> DeleteRegistrationByIdAsync(int id)
    {
        _logger.LogDebug("Deleting registration with id:{@id} from database", id);
        var reg = await _dbContext.Registration.FirstOrDefaultAsync( x => x.Id == id);
        if (reg == null) return null;

        var entity = _dbContext.Registration.Remove(reg);
        await _dbContext.SaveChangesAsync();

        if (entity == null) return null;
        return entity.Entity;
        
    }

    public async Task<Registration?> UpdateRegistrationAsync(int id, Registration registration)
    {
        _logger.LogDebug("Updating registration with id:{@id} in database", id);
        var affectedRows = await _dbContext.Registration.Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(x => x.Message, registration.Message));


        if (affectedRows == 0) return null;
        return registration;
       

    }
}

          

