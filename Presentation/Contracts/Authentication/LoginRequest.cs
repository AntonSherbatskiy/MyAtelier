using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts.Authentication;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must contains minimum 6 symbols")]
    public string Password { get; set; }
}