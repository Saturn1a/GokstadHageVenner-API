using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Repository.Interface;

public interface IMemberRepository
{
    // CREATE
    Task<Member?> AddMemberAsync(Member member);


    // READ
    Task<ICollection<Member>> GetAllMembersAsync(int page, int pageSize);
    Task<Member?> GetMemberByIdAsync(int id);
    Task<Member?> GetMemberByNameAsync(string username);


    // UPDATE
    Task<Member?> UpdateMemberAsync(int id, Member member);


    // DELETE
    Task<Member?> DeleteMemberByIdAsync(int id);
}
