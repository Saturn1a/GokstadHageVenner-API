using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Services.Interface;

public interface IRegistrationService
{
    // CREATE
    Task<RegistrationDTO?> AddRegistrationAsync(RegistrationDTO registrationDTO);

    //READ
    Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync(int page, int pageSize);
    Task<RegistrationDTO?> GetRegistrationByIdAsync(int id);


    //UPDATE
    Task<RegistrationDTO?> UpdateRegistrationAsync(int registrationId, RegistrationDTO registrationDTO, int loginId );



    // DELETE
    Task<RegistrationDTO?> DeleteRegisterAsync(int  registrationId, int loginId);


 
}
