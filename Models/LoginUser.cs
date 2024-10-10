#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;

namespace BeltReview.Models;

public class LoginUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string LoginEmail { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [Display(Name = "Password")]
    public string LoginPassword { get; set; }
}

