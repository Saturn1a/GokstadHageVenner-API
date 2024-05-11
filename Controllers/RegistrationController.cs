using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GokstadHageVennerAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IEventService _eventService;
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<MemberController> _logger;

    public RegistrationController(IMemberService memberService,
        IEventService eventService,
        IRegistrationService registrationService,
        ILogger<MemberController> logger)
    {
        _memberService = memberService;
        _eventService = eventService;
        _registrationService = registrationService;
        _logger = logger;
    }



  
    [HttpGet(Name = "GetRegistrations")]
    public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetRegistrationsAsync(int page =1, int pageSize = 10)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get all registrations");

        return Ok(await _registrationService.GetAllRegistrationsAsync(page, pageSize));

    }

    
    [HttpGet("{id}", Name = "GetRegistrationId")]
    public async Task<ActionResult<RegistrationDTO>> getRegistrationByIdAsync(int id)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get registration with id:{id}");
        var res = await _registrationService.GetRegistrationByIdAsync(id);
        return res != null ? Ok(res) : NotFound("Could not find comment");

    }

   
    [HttpPost(Name = "AddRegistration")]
    public async Task<ActionResult<RegistrationDTO>> AddRegistrationAsync(RegistrationDTO registrationDTO)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to add new registraion");
        registrationDTO.MemberId = loginId;


        var reg = await _registrationService.AddRegistrationAsync(registrationDTO);
        if (reg == null)
        {
            return BadRequest("Could not registrate to event");
        }

        return Ok(reg);
    }

    
    [HttpPut("{id}", Name = "UpdateRegistration")]
    public async Task<ActionResult<RegistrationDTO>> UpdateRegistrationAsync(int id, RegistrationDTO registrationDTO)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to update registration with id:{id}");
        var res = await _registrationService.UpdateRegistrationAsync(id, registrationDTO, loginId);


   
        return res != null ? Ok(res) : NotFound("Could not update registration");

    }


  
    [HttpDelete("{id}", Name = "DeleteRegistration")]
    public async Task<ActionResult<RegistrationDTO>> DeleteRegistraionAsync(int id)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to delete registration with id:{id}");
        var res = await _registrationService.DeleteRegisterAsync(id, loginId);
        //var res = await _commentService.DeleteCommentAsync(id);
        return res != null ? Ok(res) : BadRequest("Could not find registration to delete");

    }

    [HttpGet("{id}/registrations", Name = "GoingToEvent")]
    public async Task<ActionResult<RegistrationDTO>> GetAllCommentsPostAsync(int id, int page = 1, int pageSize = 10)
    {

        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get all registration for event with id:{id}");
        var res = await _registrationService.GetAllRegistrationsAsync(page, pageSize);

        var RegistratedMembers = res.Where(c => c.EventId == id);

        return RegistratedMembers != null ? Ok(RegistratedMembers) : BadRequest("Could not any members going to event ");

    }




}
