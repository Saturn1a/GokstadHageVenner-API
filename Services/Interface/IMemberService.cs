using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Services.Interface;

public interface IMemberService
{
    // CREATE
    Task<MemberDTO?> SignUpAsync(MemberSignUpDTO memberSignUpDTO);


    // READ
    Task <ICollection<MemberDTO>> GetAllMembersAsync(int page, int pageSize);

    Task <MemberDTO?> GetMemberByIdAsync(int id);
    Task<MemberDTO?> GetMemberByNameAsync(string username);


    // UPDATE
    Task <MemberDTO?> UpdateMemberAsync(int id, MemberDTO memberDTO, int loginId);

   

    // DELETE
    Task <MemberDTO?> DeleteMemberAsync(int id, int loginId);



    Task<int> GetAuthenticatedIdAsync(string username, string password);



}
