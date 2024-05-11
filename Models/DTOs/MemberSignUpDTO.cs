namespace GokstadHageVennerAPI.Models.DTOs;

public class MemberSignUpDTO
{
    public MemberSignUpDTO(string userName, string firstName, string lastName, string email, string password, string phoneNumber, DateTime dateOfBirth)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth.Date;
    }

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; }  = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }



}
