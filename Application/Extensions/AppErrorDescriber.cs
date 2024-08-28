using Microsoft.AspNetCore.Identity;

namespace Application.Extensions;

public class AppErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName)
    {
        var error = base.DuplicateUserName(userName);
        error.Description = "This email address has already been registered.";
        return error;
    }
}