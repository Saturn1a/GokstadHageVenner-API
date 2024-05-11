using GokstadHageVennerAPI.Mappers;
using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository;
using GokstadHageVennerAPI.Repository.Interface;
using GokstadHageVennerAPI.Services.Interface;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit.Sdk;

namespace GokstadHageVennerAPI.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly Imapper<Event, EventDTO> _eventMapper;
    private readonly ILogger<EventService> _logger;

    public EventService(IEventRepository eventRepository,
        IMemberRepository memberRepository,
        Imapper<Event, EventDTO> eventMapper,
        ILogger<EventService> logger)
    {
        _eventRepository = eventRepository;
        _memberRepository = memberRepository;
        _eventMapper = eventMapper;
        _logger = logger;
    }

    public async Task<EventDTO?> AddEventAsync(EventDTO eventDTO)
    {
        _logger.LogDebug("Mapping EventDTO to Event ");
        var gardenEvent = _eventMapper.MapToModel(eventDTO);
        var res = await _eventRepository.AddEventAsync(gardenEvent);

        if (res == null) return null;

        return eventDTO;
    }

    public async Task<EventDTO?> DeleteEventAsync(int id, int loginId)
    {
        _logger.LogDebug("Mapping Event to EventDTO ");

        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        var eventOwner = await _eventRepository.GetEventByIdAsync(id);

        if (loginMember == null) throw new UnauthorizedAccessException($"Member {loginId} is not allowed to delete event");
        if (eventOwner == null) throw new UnauthorizedAccessException($"Member {eventOwner} is not allowed to delete event");

        if (eventOwner.MemberId != loginId && !loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"User {loginId} is not allowed to delete event");
        }

        var gardenEvent = await _eventRepository.GetEventByIdAsync(id);
        if (gardenEvent == null) return null;

        var res = await _eventRepository.DeleteEventByIdAsync(id);
        return res != null ? _eventMapper.MapToDTO(gardenEvent) : null;


    }

    public async Task<IEnumerable<EventDTO>> GetAllEventsAsync(int page, int pageSize)
    {
        
        var events = await _eventRepository.GetEventsAsync(page, pageSize);

        var dtos = events.Select(gardenEvent => _eventMapper.MapToDTO(gardenEvent)).ToList();
        _logger.LogDebug($"Mapping {dtos.Count} Event to EventDTO");
        return dtos;

    }

    public async Task<EventDTO?> GetEventByIdAsync(int id)
    {
        _logger.LogDebug("Mapping Event to EventDTO ");
        var res = await _eventRepository.GetEventByIdAsync(id);
        return res != null ? _eventMapper.MapToDTO(res) : null;
    }

    public async Task<EventDTO?> UpdateEventAsync(int id, EventDTO eventDTO, int loginId)
    {
       

        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        var eventOwner = await _eventRepository.GetEventByIdAsync(id);

        if (loginMember == null) throw new UnauthorizedAccessException($"Member {loginId} is not allowed to update event");
        if (eventOwner == null) throw new UnauthorizedAccessException($"Member {eventOwner} is not allowed to update event");

        if (eventOwner.MemberId != loginId && !loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"User {loginId} is not allowed to update event");
        }

        var eventToUpdate = await _eventRepository.GetEventByIdAsync(id);
        if (eventToUpdate == null) return null;

        _logger.LogDebug($"Mapping EventDTO with id:{id} to Event");
        var gardenEvent = _eventMapper.MapToModel(eventDTO);
        gardenEvent.Id = id;

        var res = await _eventRepository.UpdateEventAsync(id, gardenEvent);
        _logger.LogDebug($"Mapping Event with id:{id} to EventDTO");
        return res != null ? _eventMapper.MapToDTO(gardenEvent) : null;



    }



}
