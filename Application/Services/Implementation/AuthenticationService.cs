using System.Security.Claims;
using Application.Common.AccountConfirmation;
using Application.ErrorModels;
using Application.Models;
using Application.Usecases.Commands.Authentication;
using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;
using IAuthenticationService = Application.Services.Interfaces.IAuthenticationService;
using Interfaces_IAuthenticationService = Application.Services.Interfaces.IAuthenticationService;

namespace Application.Services.Implementation;

public class AuthenticationService : Interfaces_IAuthenticationService 
{
    private IUnitOfWork _unit { get; set; }
    private IMapper _mapper { get; set; }
    private UserManager<ApplicationUser> _userManager { get; set; }
    private SignInManager<ApplicationUser> _signInManager { get; set; }
    private IHttpContextAccessor _context { get; set; }
    private IAsyncMessageSender _asyncMessageSender { get; set; }
    private ICodeGenerator<int> _numericCodeGenerator { get; set; }

    public AuthenticationService(
        IUnitOfWork unit, 
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor accessor,
        IAsyncMessageSender asyncMessageSender,
        ICodeGenerator<int> numericCodeGenerator)
    {
        _unit = unit;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = accessor;
        _asyncMessageSender = asyncMessageSender;
        _numericCodeGenerator = numericCodeGenerator;
    }

    public async Task<ErrorOr<UserModel>> RegisterAsync(RegisterCommand registerCommand)
    {
        if (registerCommand.Password != registerCommand.ConfirmedPassword)
        {
            return Errors.Authentication.PasswordMismatch;
        }

        var code = await _unit.UserCodeRepository.GetCodeByEmail(registerCommand.Email);

        if (code!.Code != registerCommand.ConfirmationCode)
        {
            return Errors.Authentication.IncorrectConfirmationCode;
        }
        
        var user = _mapper.Map<ApplicationUser>(registerCommand);
        var res = await _userManager.CreateAsync(user, registerCommand.Password);
        
        await _userManager.AddToRoleAsync(user, "user");
        
        if (!res.Succeeded)
        {
            return res.Errors.Select(e => Error.Conflict(description: e.Description)).ToList();
        }

        var identity = new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.Email, registerCommand.Email),
            new Claim(ClaimTypes.Role, "user"),
            new Claim("Id", user.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        await _context.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        await _unit.UserCodeRepository.RemoveAsync(code.Id);
        
        return _mapper.Map<UserModel>(user);
    }

    public async Task<ErrorOr<UserModel>> LoginAsync(LoginCommand loginCommand)
    {
        var applicationUser = await _userManager.FindByEmailAsync(loginCommand.Email);

        if (applicationUser == null)
        {
            return Errors.Authentication.UserDoesNotExist;
        }
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(applicationUser, loginCommand.Password) || applicationUser.PasswordHash is null;

        if (!isPasswordValid)
        {
            return Errors.Authentication.IncorrectPassword;
        }

        var role = (await _userManager.GetRolesAsync(applicationUser)).First();
        
        var identity = new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.Email, applicationUser.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim("Id", applicationUser.Id)
        });

        var principal = new ClaimsPrincipal(identity);

        await _context.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        
        return _mapper.Map<UserModel>(applicationUser);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<int> SendConfirmationCodeAsync(string email)
    {
        var code = _numericCodeGenerator.Generate();
        await _asyncMessageSender.SendAsync(email, "Confirm account", code.ToString());
        return code;
    }

    public async Task SaveConfirmationCodeAsync(string email, int code)
    {
       await _unit.UserCodeRepository.AddAsync(new UserCode() { Email = email, Code = code });
       _unit.Complete();
    }

    public void RemoveCodesByEmail(string userEmail)
    {
        _unit.UserCodeRepository.RemoveCodesByEmail(userEmail);
        _unit.Complete();
    }
}