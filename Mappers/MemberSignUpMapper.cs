using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Mappers;

public class MemberSignUpMapper : Imapper<Member, MemberSignUpDTO>
{
    public MemberSignUpDTO MapToDTO(Member model)
    {
        throw new NotImplementedException();
    }

    public Member MapToModel(MemberSignUpDTO dto)
    {
        var dtNow = DateTime.Now;
        return new Member()
        {
            Email = dto.Email,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            PhoneNumber = dto.PhoneNumber,
            LastName = dto.LastName,
            MemberSince = dtNow,
            IsAdminUser = false,
            DateOfBirth = dto.DateOfBirth

        };
    }
}
