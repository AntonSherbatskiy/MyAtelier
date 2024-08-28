using Librame.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Usecases.Commands.Authentication;
using Presentation.Contracts.Authentication;
using IAuthenticationService = Application.Services.Interfaces.IAuthenticationService;

namespace Presentation.Controllers.HTML;

[Route("[controller]")]
public class AuthenticationController : Controller
{
    private IMapper _mapper { get; set; }
    private IAuthenticationService _authenticationService { get; set; }

    public AuthenticationController(IMapper mapper, IAuthenticationService authenticationService)
    {
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    [HttpGet("register")]
    public IActionResult GetRegisterPage()
    {
        return View("Register");
    }

    [HttpPost("register-confirm")]
    public async Task<IActionResult> ConfirmRegister(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View("Register");
        }

        var code = await _authenticationService.SendConfirmationCodeAsync(request.Email);
        _authenticationService.RemoveCodesByEmail(request.Email);
        await _authenticationService.SaveConfirmationCodeAsync(request.Email, code);
        var confirmationCodeModel = _mapper.Map<ConfirmationCodeModel>((request, code));
        confirmationCodeModel.Code = default;

        return View("CodeConfirmation", confirmationCodeModel);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(ConfirmationCodeModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Register");
        }
        
        var command = _mapper.Map<RegisterCommand>(model);
        var res = await _authenticationService.RegisterAsync(command);
    
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("CodeConfirmation", model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("login")]
    public IActionResult GetLoginPage(string? returnUrl = null)
    {
        TempData["returnUrl"] = returnUrl;
        return View("Login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View("Login");
        }
        
        var command = _mapper.Map<LoginCommand>(request);
        var res = await _authenticationService.LoginAsync(command);
        var returnUrl = (string)TempData["returnUrl"]!;

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("Login");
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("access-denied")]
    public IActionResult GetAccessDeniedPage()
    {
        return View("AccessDenied");
    }
}