using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.HTML;

[Route("role")]
public class RolesController : Controller
{
    public IActionResult GetRole()
    {
        return Ok(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
    }
}