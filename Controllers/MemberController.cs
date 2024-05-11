using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GokstadHageVennerAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ILogger<MemberController> _logger;

    public MemberController(IMemberService memberService, 
        ILogger<MemberController> logger)
    {
        _memberService = memberService; 
        _logger = logger;
    }




    ////[HttpGet("members/{search}", Name = "GetMembersLike")]
    ////public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersLikeUsernameAsync(int page = 1, int pageSize = 10, string search = "")
    ////{
    ////    var members = await _memberService.GetAllMembersAsync(page, pageSize);

    ////    if (!string.IsNullOrEmpty(search))
    ////    {
    ////        members = members.Where(m => m.UserName.Contains(search)).ToList();
    ////    }

    ////    return Ok(members);
    ////}



    [HttpGet(Name = "GetMembers")]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersAsync(int page =1, int pageSize = 10)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get all members");
        return Ok(await _memberService.GetAllMembersAsync(page, pageSize));

    }

   
    [HttpGet("{id}", Name = "GetMemberId")]
    public async Task<ActionResult<MemberDTO>> GetMemberByIdAsync(int id)
    {
        // THIS LOG MESSAGES FAILS THE MEMBERCONTROLLER UNIT TEST
       // _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get member with id:{id}");
        var res = await _memberService.GetMemberByIdAsync(id);
        return res != null ? Ok(res) : NotFound("Could not find member");

    }

    [HttpGet("username/{username}", Name ="GetUsername")]
    public async Task <ActionResult<MemberDTO>> GetMemberByUsername(string username)
    {
        _logger.LogDebug($"MemberId:{this.HttpContext.Items?["MemberId"]} requests to get member with username:{username}");
        var res = await _memberService.GetMemberByNameAsync(username);
        return res != null ? Ok(res) : NotFound("Could not find member with that username");

    }


 
    [HttpPost("register", Name = "AddMember")]
    public async Task<ActionResult<MemberDTO>> AddUserAsync(MemberSignUpDTO memberSignupDTO)
    {
        _logger.LogDebug("New member request");
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
      
        var memberDTO = await _memberService.SignUpAsync(memberSignupDTO);
        return memberDTO != null ? Ok(memberDTO) : BadRequest("Could not add new member");


    }

    
    [HttpPut("{id}", Name = "UpdateMember")]
    public async Task<ActionResult<MemberDTO>> UpdateMemberAsync(int id, MemberDTO dto)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to update member:{id}");


        var res = await _memberService.UpdateMemberAsync(id, dto, loginId);
        return res != null ? Ok(res) : NotFound("Could not update member");

    }

    
    [HttpDelete("{id}", Name = "DeleteMember")]
    public async Task<ActionResult<MemberDTO>> DeleteMemberAsync(int id)
    {
        int loginId = (int)this.HttpContext.Items["MemberId"]!;
        _logger.LogDebug($"MemberId:{loginId} requests to delete member:{id}");

        var res = await _memberService.DeleteMemberAsync(id, loginId);
        return res != null ? Ok(res) : NotFound("Could not find member to delete");

    }
}
