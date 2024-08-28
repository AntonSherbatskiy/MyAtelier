using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Clothing;
using Presentation.Contracts.Clothing;

namespace Presentation.Controllers.HTML;

[Route("[controller]")]
public class ClothingController : Controller
{
    private IMapper _mapper { get; set; }
    private IClothingService _clothingService { get; set; }

    public ClothingController(IMapper mapper, IClothingService clothingService)
    {
        _mapper = mapper;
        _clothingService = clothingService;
    }
    
    // [HttpPost("add")]
    // public async Task<IActionResult> AddClothing(AddClothingRequest addClothingRequest)
    // {
    //     var addClothingCommand = _mapper.Map<AddClothingCommand>(addClothingRequest);
    //     var res = await _clothingService.AddClothingAsync(addClothingCommand);
    //
    //     return res.Match(
    //         onValue: Ok,
    //         onError: HandleFirstError);
    // }

    [HttpGet("remove/{id:int}")]
    public async Task<IActionResult> RemoveClothing(int id)
    {
        var res = await _clothingService.RemoveClothingByIdAsync(id);
        var clothes = await _clothingService.GetAllClothingAsync();

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
        }
        
        return View("~/Views/Admin/AdminClothingPage.cshtml", clothes);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddClothing(AddClothingCommand command)
    {
        var res = await _clothingService.AddClothingAsync(command);
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("~/Views/Admin/AdminAddClothingPage.cshtml", command);
        }

        return RedirectToAction("GetAdminClothingPage", "Admin");
    }
}