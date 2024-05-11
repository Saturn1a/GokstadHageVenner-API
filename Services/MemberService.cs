using GokstadHageVennerAPI.Data;
using GokstadHageVennerAPI.Mappers;
using GokstadHageVennerAPI.Models.DTOs;
using GokstadHageVennerAPI.Models.Entities;
using GokstadHageVennerAPI.Repository.Interface;
using GokstadHageVennerAPI.Services.Interface;

namespace GokstadHageVennerAPI.Services;

public class MemberService : IMemberService
{
    private readonly Imapper<Member, MemberDTO> _memberMapper;
    private readonly IMemberRepository _memberRepository;
    private readonly Imapper<Member, MemberSignUpDTO> _memberSignUpMapper;
    private readonly ILogger<MemberService> _logger;

    public MemberService(Imapper<Member, MemberDTO> memberMapper,
        IMemberRepository memberRepository,
        Imapper<Member, MemberSignUpDTO> memberSignUpMapper,
        ILogger<MemberService> logger)
    {
        _memberMapper = memberMapper;
        _memberRepository = memberRepository;
        _memberSignUpMapper = memberSignUpMapper;
        _logger = logger;
    }

    public async Task<MemberDTO?> DeleteMemberAsync(int id, int loginId)
    {
        _logger.LogDebug($"Mapping Member with id:{id} to MemberDTO");
        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        if (loginMember == null) throw new UnauthorizedAccessException($"User {loginId} is not allowed to update");

        if (id != loginId && !loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"User {loginId} is not allowed to delete");

        }

        var mbr = await _memberRepository.GetMemberByIdAsync(id);
        if (mbr == null) return null;

        var res = await _memberRepository.DeleteMemberByIdAsync(id);
        return res != null ? _memberMapper.MapToDTO(mbr) : null;

    }

    public async Task<ICollection<MemberDTO>> GetAllMembersAsync(int page, int pageSize)
    {
        
        var members = await _memberRepository.GetAllMembersAsync(page, pageSize);

        var dtos = members.Select(member => _memberMapper.MapToDTO(member)).ToList();
        _logger.LogDebug($"Mapping {dtos.Count} Member to MemberDTO");

        return dtos;
    }

    public async Task<int> GetAuthenticatedIdAsync(string username, string password)
    {
        _logger.LogDebug($"Authenticating member:{username}");
        var mbr = await _memberRepository.GetMemberByNameAsync(username);
        if (mbr == null) return 0;

        if (BCrypt.Net.BCrypt.Verify(password, mbr.HashedPassword))
        {
            return mbr.Id;
        }

        return 0;
        
    }

    public async Task<MemberDTO?> GetMemberByIdAsync(int id)
    {
        _logger.LogDebug($"Mapping Member with id:{id} to MemberDTO");
        var res = await _memberRepository.GetMemberByIdAsync(id);
        return res != null ? _memberMapper.MapToDTO(res) : null;
    }

    public async Task<MemberDTO?> GetMemberByNameAsync(string username)
    {
        _logger.LogDebug($"Mapping Member with username:{username} to MemberDTO");
        var res = await _memberRepository.GetMemberByNameAsync(username);
        return res != null ? _memberMapper.MapToDTO(res) : null;
    }

    public async Task<MemberDTO?> SignUpAsync(MemberSignUpDTO memberSignUpDTO)
    {
        _logger.LogDebug("Mapping MemberSignUpDTO to Member");
        var mbr = _memberSignUpMapper.MapToModel(memberSignUpDTO);

        mbr.Salt = BCrypt.Net.BCrypt.GenerateSalt();
        mbr.HashedPassword = BCrypt.Net.BCrypt.HashPassword(memberSignUpDTO.Password, mbr.Salt);

        var res = await _memberRepository.AddMemberAsync(mbr);

        _logger.LogDebug("Mapping Member to MemberDTO");
        return _memberMapper.MapToDTO(res!);
    }

    public async Task<MemberDTO?> UpdateMemberAsync(int id, MemberDTO memberDTO, int loginId)
    {
        

        var loginMember = await _memberRepository.GetMemberByIdAsync(loginId);
        if (loginMember == null) throw new UnauthorizedAccessException($"User:{loginId} is not allowed to update");

        if (id != loginId && !loginMember.IsAdminUser)
        {
            throw new UnauthorizedAccessException($"User:{loginId} is not allowed to update");

        }

        var memberToUpdate = await _memberRepository.GetMemberByIdAsync(id);
        if (memberToUpdate == null) return null;

        _logger.LogDebug($"Mapping MemberDTO with id:{id} to Member");
        var mbr = _memberMapper.MapToModel(memberDTO);
        mbr.Id = id;

        _logger.LogDebug($"Mapping Member with id:{id} to MemberDTO");
        var res = await _memberRepository.UpdateMemberAsync(id, mbr);
        return res != null ? _memberMapper.MapToDTO(mbr) : null;

         
    }
}
