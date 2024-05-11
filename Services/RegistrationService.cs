using GokstadHageVennerAPI.Mappers;
using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository.Interface;
using GokstadHageVennerAPI.Services.Interface;

namespace GokstadHageVennerAPI.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly Imapper<Registration, RegistrationDTO> _registrationMapper;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(IRegistrationRepository registrationRepository,
        IMemberRepository memberRepository,
        Imapper<Registration, RegistrationDTO> registrationMapper,
        ILogger<RegistrationService> logger)
    {
        _registrationRepository = registrationRepository;
        _memberRepository = memberRepository;
        _registrationMapper = registrationMapper;
        _logger = logger;
    }

    public async Task<RegistrationDTO?> AddRegistrationAsync(RegistrationDTO registrationDTO)
    {
        _logger.LogDebug("Mapping RegistraionDTO to Registration");


        var registarion = _registrationMapper.MapToModel(registrationDTO);
        var res = await _registrationRepository.AddRegistrationAsync(registarion);

        if (res == null)
        {
            return null;
        }

        return registrationDTO;
    }

    public async Task<RegistrationDTO?> DeleteRegisterAsync(int registrationId, int loginId)
    {
        _logger.LogDebug($"Mapping Registration with id:{registrationId} to RegistrationDTO");
        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        var regOwner = await _registrationRepository.GetRegistrationByIdAsync(registrationId);

        if (loginMember == null) throw new UnauthorizedAccessException($"{loginId} is not allowed to delete registration");
        if (regOwner == null) throw new UnauthorizedAccessException($"{regOwner} is not allowed to delete registration");

        if (regOwner.MemberId != loginId && !loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"{loginMember} is not allowed to delete registration");
        }

        var registration = await _registrationRepository.GetRegistrationByIdAsync(registrationId);
        if (registration == null) return null;

        var res = await _registrationRepository.DeleteRegistrationByIdAsync(registrationId);
        return res != null ? _registrationMapper.MapToDTO(registration) : null;
    }

    public async Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync(int page, int pageSize)
    { 
        var registrations = await _registrationRepository.GetRegistrationsAsync(page, pageSize);

        var dtos = registrations.Select( registration => _registrationMapper.MapToDTO(registration)).ToList();
        _logger.LogDebug($"Mapping {dtos.Count} Registration to RegistrationDTO");
        return dtos;
    }

    public async Task<RegistrationDTO?> GetRegistrationByIdAsync(int id)
    {
        _logger.LogDebug($"Mapping Registration with id:{id} to RegistrationDTO");
        var res = await _registrationRepository.GetRegistrationByIdAsync(id);
        return res != null ? _registrationMapper.MapToDTO(res) : null;
    }

    public async Task<RegistrationDTO?> UpdateRegistrationAsync(int registrationId, RegistrationDTO registrationDTO, int loginId)
    {
       
        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        var regOwner = await _registrationRepository.GetRegistrationByIdAsync(registrationId);

        if (loginMember == null) throw new UnauthorizedAccessException($"{loginId} is not allowed to update registration");
        if (regOwner == null) throw new UnauthorizedAccessException($"{regOwner} is not allowed to update registration");

        if (regOwner.MemberId != loginId && ! loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"{loginMember} is not allowed to update registration");
        }

        var regToUpdate = await _registrationRepository.GetRegistrationByIdAsync(registrationId);
        if (regToUpdate == null) return null;

        _logger.LogDebug($"Mapping RegistraionDTO with id:{registrationId} to Registration");
        var registration = _registrationMapper.MapToModel(registrationDTO);
        registration.Id = registrationId;

        var res = await _registrationRepository.UpdateRegistrationAsync(registrationId, registration);

        _logger.LogDebug($"Mapping Registraion with id:{registrationId} to RegistrationDTO");
        return res != null ? _registrationMapper.MapToDTO(registration) : null;

    }
}
