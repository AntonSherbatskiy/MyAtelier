using System.Security.Claims;
using Application.ErrorModels;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.User;
using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Implementation;

public class UserService : IUserService
{
    private IUnitOfWork _unit { get; set; }
    private IMapper _mapper { get; set; }
    private UserManager<ApplicationUser> _userManager { get; set; }
    private IHttpContextAccessor _contextAccessor { get; set; }
    private AppDbContext _context { get; set; }

    public UserService(IUnitOfWork unit, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, AppDbContext context)
    {
        _unit = unit;
        _mapper = mapper;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _context = context;
    }

    public async Task<ErrorOr<UserModel>> AddUserAsync(AddUserCommand addUserCommand)
    {
        var existedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == addUserCommand.Email);
        
        if (existedUser != null)
        {
            return Errors.Authentication.UserAlreadyExists;
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == addUserCommand.RoleName);
        
        if (role == null)
        {
            return Errors.Role.InvalidRole;
        }
        
        var userModel = _mapper.Map<UserModel>((role, addUserCommand));
        var user = new ApplicationUser()
        {
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            UserName = userModel.Email
        };

        var res = await _userManager.CreateAsync(user, addUserCommand.Password);
        
        if (!res.Succeeded)
        {
            return res.Errors.Select(e => Error.Conflict(description: e.Description)).ToList();
        }
        
        await _userManager.AddToRoleAsync(user, role.Name);

        _unit.Complete();
        
        return userModel;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _userManager.Users.Select(u => new UserModel()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
        }).ToListAsync();
        
        users.ForEach(u =>
        {
            u.RoleName = GetRoleByUserIdAsync(u.Id).Name;
        });
        
        return users;
    }

    public async Task<ErrorOr<bool>> RemoveUserAsync(RemoveUserCommand removeUserCommand)
    {
        var userId = removeUserCommand.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
        var orders = await _unit.OrderRepository.GetOrdersByUserIdAsync(userId);

        if (orders.Any(o => o.Status == "Process"))
        {
            return Errors.User.SomeOrdersAreProcess;
        }

        var applicationUser = await _userManager.GetUserByClaimsPrincipalEmail(removeUserCommand.User);
        var res = await _userManager.DeleteAsync(applicationUser);

        if (!res.Succeeded)
        {
            return (dynamic)res.Errors.Select(e => Error.Failure(description: e.Description));
        }

        await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return true;
    }

    public async Task<ErrorOr<UserModel>> GetUserByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            return Errors.Authentication.UserDoesNotExist;
        }

        return _mapper.Map<UserModel>(user);
    }

    public async Task<ErrorOr<UserModel>> UpdateUserAsync(UpdateUserModel model, bool isAdmin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        
        if (user.Email != model.Email)
        {
            var emailUser = await _userManager.FindByEmailAsync(model.Email);
            if (emailUser != null)
            {
                return Errors.Authentication.UserAlreadyExists;
            }
            
            user.Email = model.Email;
            user.UserName = model.Email;

            if (!isAdmin)
            {
                await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
        
        if (!string.IsNullOrEmpty(model.Password))
        {
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            
            if (!isAdmin)
            {
                await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
        
        await _userManager.UpdateAsync(user);

        return _mapper.Map<UserModel>(user);
    }

    public async Task<ErrorOr<UserModel>> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return Errors.Authentication.UserDoesNotExist;
        }

        var role = GetRoleByUserIdAsync(id);

        return _mapper.Map<UserModel>((user, role.Name));
    }

    private IdentityRole GetRoleByUserIdAsync(string userId)
    {
        var roleId = _context.UserRoles.FirstOrDefault(r => r.UserId == userId).RoleId;
        return _context.Roles.FirstOrDefault(r => r.Id == roleId);
    }
}