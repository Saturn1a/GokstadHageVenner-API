using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

 namespace GokstadHageVennerAPI.Models.Entities;

public class Event
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("MemberId")]
    public int MemberId { get; set; }

    [Required]
    public string EventName { get; set; } = string.Empty;

    public string EventDescription { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }




    // navigation properties
    public virtual Member? Member { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();





}
