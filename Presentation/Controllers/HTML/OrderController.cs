using System.Security.Claims;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Order;

namespace Presentation.Controllers.HTML;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "user, manager, admin")]
[Route("orders")]
public class OrderController : Controller
{
    private IMapper _mapper { get; set; }
    private IFavorService _favorService { get; set; }
    private IOrderService _orderService { get; set; }

    public OrderController(IMapper mapper, IFavorService favorService, IOrderService orderService)
    {
        _mapper = mapper;
        _favorService = favorService;
        _orderService = orderService;
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "user")]
    [HttpPost("repairing")]
    public async Task<IActionResult> PlaceRepairingServiceOrder(PlaceRepairingOrderModel model)
    {
        var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        var res = await _orderService.PlaceOrderAsync(_mapper.Map<PlaceOrderCommand>(model), userEmail);

        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("RepairingServiceDetails");
        }
        
        return RedirectToAction("GetUserOrdersPage");
    }
    
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "user")]
    [HttpPost("sewing")]
    public async Task<IActionResult> PlaceSewingServiceOrder(PlaceSewingOrderModel model)
    {
        var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        var res = await _orderService.PlaceOrderAsync(_mapper.Map<PlaceOrderCommand>(model), userEmail);
        
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            return View("SewingServiceDetails");
        }
        
        return RedirectToAction("GetUserOrdersPage");
    }

    [HttpGet("repairing/{id}")]
    public async Task<IActionResult> GetRepairingServiceDetailsPage(int id)
    {
        var res = await _favorService.GetRepairingServiceByIdAsync(id);
        var placeRepairingOrder = _mapper.Map<PlaceRepairingOrderModel>(res.Value);

        return View("RepairingServiceDetails", placeRepairingOrder);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserOrdersPage()
    {
        var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
        var orders = await _orderService.GetUserOrdersByUserEmailAsync(userEmail);
    
        return View("UserOrders", orders.Reverse());
    }

    [HttpGet("sewing/{id}")]
    public async Task<IActionResult> GetSewingServiceDetailsPage(int id)
    {
        var res = await _favorService.GetSewingServiceByIdAsync(id);
        var placeSewingOrder = _mapper.Map<PlaceSewingOrderModel>(res.Value);

        return View("SewingServiceDetails", placeSewingOrder);
    }

    [HttpGet("details/{serviceType}/{id:int}")]
    public async Task<IActionResult> GetOrderDetailsPage(string serviceType, int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        
        if (serviceType.ToLower() == "repairing")
        {
            var repairingOrderDetails = _mapper.Map<RepairingOrderDetails>(order);
            return View("RepairingOrderDetails", repairingOrderDetails);
        }
        
        var sewingServiceOrderDetails = _mapper.Map<SewingOrderDetails>(order);
        return View("SewingOrderDetails", sewingServiceOrderDetails);
    }

    [HttpGet("complete/{id:int}")]
    public async Task<IActionResult> CompleteOrder(int id)
    {
        var res = await _orderService.CompleteOrder(id);
        
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            var orders = await _orderService.GetAllOrdersAsync();
            return View("~/Views/Admin/AdminOrderPage.cshtml", orders);
        }

        return View("~/Views/Admin/AdminOrderPage.cshtml", res.Value);
    }

    [HttpGet("cancel/{id:int}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var res = await _orderService.CancelOrderAsync(id);
        
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            var orders = await _orderService.GetAllOrdersAsync();
            return View("~/Views/Admin/AdminOrderPage.cshtml", orders);
        }

        return View("~/Views/Admin/AdminOrderPage.cshtml", res.Value);
    }

    [HttpGet("remove/{id:int}")]
    public async Task<IActionResult> RemoveOrder(int id)
    {
        var res = await _orderService.DeleteOrderByIdAsync(id);
        
        if (res.IsError)
        {
            res.Errors.ForEach(e => ModelState.AddModelError("", e.Description));
            var orders = await _orderService.GetAllOrdersAsync();
            return View("~/Views/Admin/AdminOrderPage.cshtml", orders);
        }

        return View("~/Views/Admin/AdminOrderPage.cshtml", await _orderService.GetAllOrdersAsync());
    }
}