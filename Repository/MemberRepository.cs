using GokstadHageVennerAPI.Data;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GokstadHageVennerAPI.Repository;

public class MemberRepository : IMemberRepository
{
    private readonly GokstadHageVennerDbContext _dbContext;
    private readonly ILogger<MemberRepository> _logger;

    public MemberRepository(GokstadHageVennerDbContext dbContext, ILogger<MemberRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Member?> AddMemberAsync(Member member)
    {
        _logger.LogDebug("Adding member to database");

        var addedMbr = await _dbContext.AddAsync(member);
        await _dbContext.SaveChangesAsync();

        if (addedMbr != null)
        {
            return addedMbr.Entity;
        }

       
        return null;
    }

    public async Task<Member?> DeleteMemberByIdAsync(int id)
    {
        _logger.LogDebug("Deleting member with id:{@id} from database", id);
        var mbr = await _dbContext.Members.FirstOrDefaultAsync( x => x.Id == id);
        if (mbr == null) return null;

        var deletedMbr = _dbContext.Members.Remove(mbr);
        await _dbContext.SaveChangesAsync();

        if (deletedMbr != null)
        {
            return deletedMbr.Entity;
        }

        return null;

     
      
    }

    public async Task<ICollection<Member>> GetAllMembersAsync(int page, int pageSize)
    {
        _logger.LogDebug("Getting all members from database");
        var totCount = _dbContext.Members.Count();
        if (totCount == 0)
        {
            return Enumerable.Empty<Member>().ToList();
        }

        return await _dbContext.Members
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        _logger.LogDebug("Getting member with id:{@id} from database", id);
        var mbr = await (_dbContext.Members.SingleOrDefaultAsync( x => x.Id == id));
        return mbr is null? null : mbr;

    }

    public async Task<Member?> GetMemberByNameAsync(string username)
    {
       _logger.LogDebug("Getting member with username:{@username} from database", username);
        var mbr = await _dbContext.Members.FirstOrDefaultAsync(x => x.UserName.Equals(username));
        return mbr is null? null : mbr;

       
    }

    public async Task<Member?> UpdateMemberAsync(int id, Member member)
    {
        _logger.LogDebug("Updating member with id:{@id} in database", id);
        var affectedRows = await _dbContext.Members.Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(x => x.UserName, member.UserName)
            .SetProperty(x => x.FirstName, member.FirstName)
            .SetProperty(x => x.LastName, member.LastName)
            .SetProperty(x => x.PhoneNumber, member.PhoneNumber)
            .SetProperty(x => x.Email, member.Email));

        if (affectedRows == 0 ) return null;
        return member;


    }
}

