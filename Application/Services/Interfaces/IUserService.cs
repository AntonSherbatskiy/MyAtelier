using Application.Models;
using Application.Usecases.Commands.User;
using ErrorOr;
using Application.Usecases.Commands;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<ErrorOr<UserModel>> AddUserAsync(AddUserCommand addUserCommand);
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<ErrorOr<bool>> RemoveUserAsync(RemoveUserCommand removeUserCommand);
    Task<ErrorOr<UserModel>> GetUserByEmailAsync(string userEmail);
    Task<ErrorOr<UserModel>> UpdateUserAsync(UpdateUserModel model, bool isAdmin);
    Task<ErrorOr<UserModel>> GetUserByIdAsync(string id);
}