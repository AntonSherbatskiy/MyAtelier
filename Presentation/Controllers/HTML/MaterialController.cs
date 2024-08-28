using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Material;

namespace Presentation.Controllers.HTML;

[Route("[controller]")]
public class MaterialController : Controller
{
    private IMaterialService _materialService { get; set; }
    
    public MaterialController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMaterial(MaterialModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/AdminUpdateMaterialPage.cshtml", model);
        }
        
        var command = new UpdateMaterialCommand()
        {
            Id = model.Id,
            Quantity = model.Quantity
        };

        var res = await _materialService.UpdateMaterialAsync(command);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("~/Views/Admin/AdminUpdateMaterialPage.cshtml", model);
        }

        return RedirectToAction("GetAdminWarehousePage", "Admin");
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMaterialPage(int id)
    {
        var material = await _materialService.GetMaterialByIdAsync(id);

        return View("~/Views/Admin/AdminUpdateMaterialPage.cshtml", material.Value);
    }

    [HttpGet("remove/{id:int}")]
    public async Task<IActionResult> RemoveMaterial(int id)
    {
        var res = await _materialService.RemoveMaterialByIdAsync(id);
        var materials = await _materialService.GetAllMaterialsAsync();

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
        }

        return View("~/Views/Admin/AdminMaterialsPage.cshtml", materials);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMaterial(MaterialModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/AdminAddMaterialPage.cshtml", model);
        }
        
        var command = new AddMaterialCommand()
        {
            MaterialName = model.Name,
            Quantity = model.Quantity
        };

        var res = await _materialService.AddMaterialAsync(command);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("~/Views/Admin/AdminAddMaterialPage.cshtml", model);
        }

        return RedirectToAction("GetAdminWarehousePage", "Admin");
    }
}