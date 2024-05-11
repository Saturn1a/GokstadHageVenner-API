using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GokstadHageVennerAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IEventService _eventService;
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<MemberController> _logger;

    public EventController(IMemberService memberService,
        IEventService eventService,
        IRegistrationService registrationService,
        ILogger<MemberController> logger)
    {
        _memberService = memberService;
        _eventService = eventService;
        _registrationService = registrationService;
        _logger = logger;
    }




  
    [HttpGet("{search}", Name = "GetEventsLike")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsLikeAsync(int page = 1, int pageSize = 10, string search = "")
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get events like:{search}");

        var events = await _eventService.GetAllEventsAsync(page, pageSize);

        if (!string.IsNullOrEmpty(search))
        {
            events = events.Where(m => m.EventName.Contains(search)).ToList();
        }

        return events != null ? Ok(events) : NotFound($"Could not find any events like:{search}");

    }

    [HttpGet(Name = "GetEvents")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsAsync(int page =1 , int pageSize= 10)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get all events");
        return Ok(await _eventService.GetAllEventsAsync(page, pageSize));

    }


 
    [HttpGet("id/{id}", Name = "GetEventId")]
    public async Task<ActionResult<EventDTO>> getEventsById(int id)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get event with id:{id}");
        var res = await _eventService.GetEventByIdAsync(id);
        return res != null ? Ok(res) : NotFound($"Could not find event with id {id}");

    }

   
    [HttpPost(Name = "AddEvent")]
    public async Task<ActionResult<EventDTO>> AddpostAsync(EventDTO eventDTO)
    {

        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to add new event");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        eventDTO.MemberId = loginId;
        var gardenEvent = await _eventService.AddEventAsync(eventDTO);
        if (gardenEvent == null)
        {
            return BadRequest("Could not add new event");
        }

        return Ok(gardenEvent);
    }

  
    [HttpPut("{id}", Name = "UpdateEvent")]
    public async Task<ActionResult<EventDTO>> UpdateEventAsync(int id, EventDTO dto)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to update event with id:{id}");

        var res = await _eventService.UpdateEventAsync(id, dto, loginId);
        return res != null ? Ok(res) : NotFound("Could not find event to update");


    }

  
    [HttpDelete("{id}", Name = "DeleteEvent")]
    public async Task<ActionResult<EventDTO>> DeleteEventAsync(int id)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to delete event with id:{id}");

        var res = await _eventService.DeleteEventAsync(id, loginId);
        return res != null ? Ok(res) : NotFound("Could not find event to delete");

    }

    
    [HttpGet("{Id}/Events", Name = "EventsByMember")]
    public async Task<ActionResult<EventDTO>> GetEvensByMemberAsync(int Id, int page = 1, int pageSize = 10)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get all events from memberId:{Id}");
        var res = await _eventService.GetAllEventsAsync(page, pageSize);

        var eventsByMember = res.Where(p => p.MemberId == Id);

        return eventsByMember != null ? Ok(eventsByMember) : NotFound("Could not find any events by that member");

    }


}
