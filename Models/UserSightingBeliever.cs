using System.ComponentModel.DataAnnotations;

namespace BeltReview.Models;

public class UserSightingBeliever
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int SightingId { get; set; }
    public Sighting? Sighting { get; set; }

}