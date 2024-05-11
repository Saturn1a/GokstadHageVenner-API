namespace GokstadHageVennerAPI.Models.DTOs;

public class MemberDTO
{
    public MemberDTO(int id, string firstName, string lastName, string userName, string email, DateTime memberSince, string phoneNumber, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        MemberSince = memberSince;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth.Date;
    }

    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime MemberSince { get; set; }

    public string PhoneNumber {  get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; } 
}
