using System.Security.Claims;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Extensions;
using Application.Models;
using Application.Services.Interfaces;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;
using IAuthenticationService = Application.Services.Interfaces.IAuthenticationService;

namespace Presentation.Controllers.HTML;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "user, admin, manager")]
[Route("user")]
public class UserController : Controller
{
    private IUserService _userService { get; set; }
    private IMapper _mapper { get; set; }
    private IOrderService _orderService { get; set; }
    private IAuthenticationService _authenticationService { get; set; }
    private IUnitOfWork _unit { get; set; }
    private UserManager<ApplicationUser> _userManager { get; set; }

    public UserController(
        IUserService userService, 
        IMapper mapper, 
        IOrderService orderService,
        IAuthenticationService authenticationService,
        IUnitOfWork unit,
        UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _mapper = mapper;
        _orderService = orderService;
        _authenticationService = authenticationService;
        _unit = unit;
        _userManager = userManager;

    }
    
    [HttpGet("info")]
    public async Task<IActionResult> GetUserInfoPageAsync()
    {
        var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        
        var res = await _userService.GetUserByEmailAsync(userEmail);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return RedirectToAction("Logout", "Authentication");
        }

        var updateUserModel = _mapper.Map<UpdateUserModel>(res.Value);
        return View("UserInfo", updateUserModel);
    }

    [HttpGet("get-code/{email}")]
    public async Task GetConfirmationCode(string email)
    {
        var code = await _authenticationService.SendConfirmationCodeAsync(email);
        _authenticationService.RemoveCodesByEmail(email);
        await _authenticationService.SaveConfirmationCodeAsync(email, code);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateUser(UpdateUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("UserInfo", model);
        }

        var code = await _unit.UserCodeRepository.GetCodeByEmail(model.Email);
        
        if (code.Code != model.ConfirmationCode)
        {
            ModelState.AddModelError("", "Incorrect confirmation code");
            return View("UserInfo", model);
        }

        var res = await _userService.UpdateUserAsync(model, false);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("UserInfo", model);
        }

        return View("UserInfo", model);
    }

    [HttpGet("remove-account/{email}")]
    public async Task<IActionResult> RemoveAccount(string email)
    {
        var orders = await _orderService.GetUserOrdersByUserEmailAsync(email);
        
        if (orders.Any(o => o.Status.ToLower() == "process"))
        {
            ModelState.AddModelError("", "Cannot remove account with unfinished orders");
            var userToUpdate = (await _userService.GetUserByEmailAsync(email)).Value;
            
            return View("UserInfo", _mapper.Map<UpdateUserModel>(userToUpdate));
        }

        var user = await _userManager.GetUserByClaimsPrincipalEmail(HttpContext.User);
        await _userManager.DeleteAsync(user);
        await HttpContext.SignOutAsync();

        return RedirectToAction("Register", "Authentication");
    }
}