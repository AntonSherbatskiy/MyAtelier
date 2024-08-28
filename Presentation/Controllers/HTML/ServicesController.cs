using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Services.Repairing;
using Application.Usecases.Commands.Services.Sewing;

namespace Presentation.Controllers.HTML;

[Route("services")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "admin, user, manager")]
public class ServicesController : Controller
{
    public ServicesController(IFavorService favorService)
    {
        _favorService = favorService;
    }

    private IFavorService _favorService { get; set; }
    
    [HttpGet("repairing")]
    public async Task<IActionResult> GetRepairingServicesPage()
    {
        var repairingServices = await _favorService.GetAllRepairingServicesAsync();
        return View("Repairing/RepairingServices", repairingServices);
    }

    [HttpGet("sewing")]
    public async Task<IActionResult> GetSewingServicesPage()
    {
        var sewingServices = await _favorService.GetAllSewingServicesAsync();
        return View("Sewing/SewingServices", sewingServices);
    }

    [HttpGet("sewing-groups")]
    public async Task<IActionResult> GetSewingServicesGroupedPage()
    {
        var sewingServicesGroups = await _favorService.GetSewingServiceGroupsAsync();
        return View("Sewing/SewingServicesGrouped", sewingServicesGroups);
    }

    [HttpGet("sewing/{clothingName}")]
    public async Task<IActionResult> GetSewingServicesInGroupPage(string clothingName)
    {
        var sewingServicesGroup = await _favorService.GetSewingServicesInGroupAsync(clothingName);
        return View("Sewing/SewingServicesInGroup", sewingServicesGroup);
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "manager, admin")]
    [HttpGet("repairing/remove/{id:int}")]
    public async Task<IActionResult> RemoveRepairingService(int id)
    {
        var repairingServices = await _favorService.GetAllRepairingServicesAsync();
        var res = await _favorService.RemoveRepairingServiceAsync(new RemoveRepairingServiceCommand()
        {
            Id = id
        });

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("~/Views/Admin/AdminRepairingServicesPage.cshtml", repairingServices);
        }

        return RedirectToAction("GetAdminRepairingServicesPage", "Admin");
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "manager, admin")]
    [HttpGet("remove/sewing/{id:int}")]
    public async Task<IActionResult> RemoveSewingService(int id)
    {
        var sewingServices
            = await _favorService.GetAllSewingServicesAsync();
        
        var res = await _favorService.RemoveSewingServiceAsync(new RemoveSewingServiceCommand()
        {
            Id = id
        });

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("~/Views/Admin/AdminSewingServicesPage.cshtml", sewingServices);
        }

        return RedirectToAction("GetAdminSewingServicesPage", "Admin");
    }
}