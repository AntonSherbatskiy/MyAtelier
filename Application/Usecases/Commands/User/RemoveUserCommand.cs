using System.Security.Claims;

namespace Application.Usecases.Commands.User;

public class RemoveUserCommand
{
    public ClaimsPrincipal User { get; set; }
}