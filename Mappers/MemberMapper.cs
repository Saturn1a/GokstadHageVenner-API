using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Mappers;

public class MemberMapper : Imapper<Member, MemberDTO>
{
    public MemberDTO MapToDTO(Member model)
    {
        return new MemberDTO(model.Id, model.FirstName, model.LastName, model.UserName, model.Email, model.MemberSince, model.PhoneNumber, model.DateOfBirth);
    }

    public Member MapToModel(MemberDTO dto)
    {
        return new Member()
        {

            Id = dto.Id,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            MemberSince = dto.MemberSince
        };
    }
}

