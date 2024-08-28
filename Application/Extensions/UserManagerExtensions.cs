using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyAtelier.DAL.Entities;

namespace Application.Extensions;

public static class UserManagerExtensions
{
    public static async Task<ApplicationUser?> GetUserByClaimsPrincipalEmail(
        this UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

        if (email == null)
        {
            return null;
        }

        return await userManager.FindByEmailAsync(email.Value);
    }
}