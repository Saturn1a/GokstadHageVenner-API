using GokstadHageVennerAPI.Models.Entities;

namespace GokstadHageVennerAPI.Repository.Interface;

public interface IRegistrationRepository
{
    //CREATE
    Task<Registration?> AddRegistrationAsync(Registration registration);



    //READ
    Task<ICollection<Registration>> GetRegistrationsAsync(int page, int pageSize);
    Task<Registration?> GetRegistrationByIdAsync(int id);



    // UPDATE
    Task<Registration?> UpdateRegistrationAsync(int id, Registration registration);




    // DELETE
    Task<Registration?> DeleteRegistrationByIdAsync(int id);
}
