using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GokstadHageVennerAPI.Models.Entities;

public class Registration
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("EventId")]
    public int EventId { get; set; }

    [ForeignKey("MemberId")]
    public int MemberId { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;




    // Navigation properties
    public virtual Member? Member { get; set; }
    public virtual Event? Event { get; set; }










}
