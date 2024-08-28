using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Authentication;
using Application.Usecases.Commands.Services.Repairing;
using Application.Usecases.Commands.Services.Sewing;
using Application.Usecases.Commands.User;
using MyAtelier.DAL.Entities;

namespace Presentation.Controllers.HTML;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin, manager")]
[Route("[controller]")]
public class AdminController : Controller
{
    private IFavorService _favorService { get; set; }
    private IOrderService _orderService { get; set; }
    private IClothingService _clothingService { get; set; }
    private IMaterialService _materialService { get; set; }
    private IUserService _userService { get; set; }
    private UserManager<ApplicationUser> _userManager { get; set; }

    public AdminController(
        IFavorService favorService, 
        IOrderService orderService, 
        IClothingService clothingService,
        IMaterialService materialService,
        IUserService userService,
        UserManager<ApplicationUser> userManager)
    {
        _favorService = favorService;
        _orderService = orderService;
        _clothingService = clothingService;
        _materialService = materialService;
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult GetAdminPage()
    {
        return View("AdminPage");
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin")]
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAdminAccountsPage()
    {
        var users = await _userService.GetAllAsync();
        return View("AdminAccountsPage", users);
    }

    [HttpGet("services")]
    public IActionResult GetAdminServicesPage()
    {
        return View("AdminServicesPage");
    }

    [HttpGet("warehouse")]
    public async Task<IActionResult> GetAdminWarehousePage()
    {
        var materials = await _materialService.GetAllMaterialsAsync();
        return View("~/Views/Admin/AdminMaterialsPage.cshtml", materials);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetAdminOrdersPage()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View("AdminOrderPage", orders);
    }

    [HttpGet("services/repairing")]
    public async Task<IActionResult> GetAdminRepairingServicesPage()
    {
        var repairingServices = await _favorService.GetAllRepairingServicesAsync();
        return View("AdminRepairingServicesPage", repairingServices);
    }

    [HttpGet("services/sewing")]
    public async Task<IActionResult> GetAdminSewingServicesPage()
    {
        var sewingServices = await _favorService.GetAllSewingServicesAsync();
        return View("AdminSewingServicesPage", sewingServices);
    }

    [HttpGet("services/repairing/page")]
    public IActionResult GetAdminAddRepairingServicePage()
    {
        return View("AdminAddRepairingServicePage");
    }
    
    [HttpGet("clothing")]
    public async Task<IActionResult> GetAdminClothingPage()
    {
        var clothes = await _clothingService.GetAllClothingAsync();
        return View("AdminClothingPage", clothes);
    }

    [HttpGet("clothing/add")]
    public IActionResult GetAdminAddClothingPage()
    {
        return View("~/Views/Admin/AdminAddClothingPage.cshtml");
    }

    [HttpGet("material/add")]
    public IActionResult GetAdminAddMaterialPage()
    {
        return View("AdminAddMaterialPage");
    }

    [HttpGet("services/sewing/page")]
    public IActionResult GetAdminAddSewingServicePage()
    {
        return View("~/Views/Admin/AdminAddSewingServicePage.cshtml");
    }

    [HttpPost("services/sewing")]
    public async Task<IActionResult> AddSewingService(AddSewingServiceCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View("AdminAddSewingServicePage", command);
        }

        var res = await _favorService.AddSewingServiceAsync(command);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("AdminAddSewingServicePage", command);
        }

        return RedirectToAction("GetAdminSewingServicesPage");
    }
    
    [HttpPost("services/repairing")]
    public async Task<IActionResult> AddRepairingService(AddRepairingServiceCommand command)
    {
        if (!ModelState.IsValid)
        {
            return View("AdminAddRepairingServicePage", command);
        }
        
        var res = await _favorService.AddRepairingServiceAsync(command);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("AdminAddRepairingServicePage", command);
        }

        return RedirectToAction("GetAdminRepairingServicesPage");
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetAdminUpdateAccountPage(string id)
    {
        var res = await _userService.GetUserByIdAsync(id);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            var users = await _userService.GetAllAsync();
            return View("AdminAccountsPage", users);
        }

        return View("AdminUpdateAccountPage", res.Value);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateUser(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("AdminUpdateAccountPage", model);
        }
        
        var res = await _userService.UpdateUserAsync(new UpdateUserModel()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password
        }, true);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("AdminUpdateAccountPage", model);
        }

        return RedirectToAction("GetAdminAccountsPage");
    }
    
    [HttpGet("user/orders/{id}")]
    public async Task<IActionResult> GetUserOrdersPage(string id)
    {
        var userEmail = (await _userService.GetUserByIdAsync(id)).Value.Email;
        var orders = await _orderService.GetUserOrdersByUserEmailAsync(userEmail);
    
        return View("~/Views/Order/UserOrders.cshtml", orders.Reverse());
    }

    [HttpGet("user/remove/{id}")]
    public async Task<IActionResult> RemoveAccount(string id)
    {
        var user = (await _userService.GetUserByIdAsync(id)).Value;
        if (user.RoleName == "admin")
        {
            ModelState.AddModelError("", "Cannot remove admin");
            var users = await _userService.GetAllAsync();
            return View("AdminAccountsPage", users);
        }
        var orders = await _orderService.GetUserOrdersByUserEmailAsync(user.Email);
        
        if (orders.Any(o => o.Status.ToLower() == "process"))
        {
            ModelState.AddModelError("", "Cannot remove account with unfinished orders");
            var users = await _userService.GetAllAsync();
            
            return View("AdminAccountsPage", users);
        }

        await _userManager.DeleteAsync(await _userManager.FindByIdAsync(id));

        return RedirectToAction("GetAdminAccountsPage");
    }

    [HttpPost("user/add")]
    public async Task<IActionResult> AddAccount(AddUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("AdminAddUserPage", model);
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            ModelState.AddModelError("", "Password cannot be empty");
            return View("AdminAddUserPage", model);
        }
        
        var res = await _userService.AddUserAsync(new AddUserCommand()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            RoleName = model.RoleName
        });
    
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("AdminAddUserPage", model);
        }

        return RedirectToAction("GetAdminAccountsPage");
    }

    [HttpGet("user/add")]
    public IActionResult GetAdminAddUserPage()
    {
        return View("AdminAddUserPage");
    }
}