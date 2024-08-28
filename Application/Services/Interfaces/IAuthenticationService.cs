using Application.Models;
using Application.Usecases.Commands.Authentication;
using ErrorOr;

namespace Application.Services.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<UserModel>> RegisterAsync(RegisterCommand registerCommand);
    Task<ErrorOr<UserModel>> LoginAsync(LoginCommand loginCommand);
    Task LogoutAsync();
    Task<int> SendConfirmationCodeAsync(string email);
    Task SaveConfirmationCodeAsync(string email, int code);
    void RemoveCodesByEmail(string userEmail);
}