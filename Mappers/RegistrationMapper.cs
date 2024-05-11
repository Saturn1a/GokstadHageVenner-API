using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Mappers;

public class RegistrationMapper : Imapper<Registration, RegistrationDTO>
{
    public RegistrationDTO MapToDTO(Registration model)
    {
        return new RegistrationDTO(model.Id, model.EventId, model.MemberId, model.Message);
    }

    public Registration MapToModel(RegistrationDTO dto)
    {
        return new Registration()
        {
            Id = dto.Id,
            EventId = dto.EventId,
            MemberId = dto.MemberId,
            Message = dto.Message


        };
    }
}
