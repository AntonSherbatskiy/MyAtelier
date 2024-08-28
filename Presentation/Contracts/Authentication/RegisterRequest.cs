using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Authentication;

public class RegisterRequest
{
    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "Minimum length is {1}")]
    [MaxLength(50, ErrorMessage = "Maximum length is {1}")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Min length is {1}")]
    [MaxLength(75, ErrorMessage = "Maximum length is {1}")]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must contains {1} symbols")]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords must be equals")]
    public string ConfirmedPassword { get; set; }
}