namespace Application.Models;

public class AuthenticationResult
{
    public UserModel UserModel { get; set; }
    public string Token { get; set; }
}