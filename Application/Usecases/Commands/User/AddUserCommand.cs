namespace Application.Usecases.Commands.User;

public class AddUserCommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RoleName { get; set; }
}