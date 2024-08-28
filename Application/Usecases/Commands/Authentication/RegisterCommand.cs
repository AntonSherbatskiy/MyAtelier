namespace Application.Usecases.Commands.Authentication;

public class RegisterCommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int ConfirmationCode { get; set; }
    public string ConfirmedPassword { get; set; }
}