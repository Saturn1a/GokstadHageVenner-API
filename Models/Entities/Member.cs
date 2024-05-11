using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace GokstadHageVennerAPI.Models.Entities;

public class Member
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3), MaxLength(20)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MinLength(8), MaxLength(8)]
    public string PhoneNumber {  get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; } 


    [Required]
    public DateTime MemberSince { get; set; }

    [Required]
    public bool IsAdminUser { get; set;}

    public string HashedPassword { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;



    // Navigation properties
    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    public virtual ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();
}
