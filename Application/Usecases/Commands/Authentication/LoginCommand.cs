namespace Application.Usecases.Commands.Authentication;

public class LoginCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsRemember { get; set; } = false;
}