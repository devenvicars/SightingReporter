#pragma warning disable CS8618
#pragma warning disable CS8602
#pragma warning disable CS8600

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeltReview.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [Display(Name = "User Name")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [UniqueEmail] //comes from custom validation
    [Display(Name = "Email")]

    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password")]
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Sighting> OwnedSightings { get; set; } = new();
    public List<UserSightingBeliever> Believers { get; set; } = new();


}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

        if (_context.Users.Any(u => u.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}
