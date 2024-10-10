#pragma warning disable CS8618
#pragma warning disable CS8603

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeltReview.Models;

public class Sighting
{
    [Key]
    public int SightingId { get; set; }

    [Required]
    [MinLength(3)]
    public string Title { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    [PastDate]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime FoundDate { get; set; }

    [Required]
    [MinLength(30)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public int OwnerId { get; set; }
    public User? Owner { get; set; }

    public List<UserSightingBeliever> Believers { get; set; } = new();
}

public class PastDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if ((DateTime)value! > DateTime.Today)
        {
            return new ValidationResult("Must be past date");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}
