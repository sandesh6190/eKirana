using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Manager.Interfaces;
using eKirana.ViewModels.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace eKirana.Controllers;
public class AuthenticationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthManager _authmanager;
    private readonly INotyfService _notification;
    public AuthenticationController(ApplicationDbContext context, IAuthManager authManager, INotyfService notyfService)
    {
        _context = context;
        _authmanager = authManager;
        _notification = notyfService;

    }
    public IActionResult LogIn()
    {
        var vm = new LogInVm();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInVm vm)
    {
        if (!ModelState.IsValid)
        {
            _notification.Warning("Invalid Input.");
            return View(vm);
        }

        try
        {
            await _authmanager.LogIn(vm.Email, vm.PassWord);
            _notification.Success("Logged In Successfully.");
            return RedirectToAction("Index", "DashBoard");
        }
        catch (Exception e)
        {
            vm.ErrorMessage = e.Message;
            return View(vm);
        }
    }

    public IActionResult Register()
    {
        var vm = new RegisterVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid)
        {
            _notification.Warning("Invalid Input.");
            return View(vm);

        }

        try
        {
            await _authmanager.Register(vm.UserName, vm.Email, vm.PassWord);
            _notification.Success("Admin Registered Successfully.");
            return RedirectToAction("LogIn");
        }
        catch (Exception e)
        {
            vm.ErrorMessage = e.Message;
            _notification.Error(e.Message);
            return View(vm);
        }
    }
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _authmanager.LogOut();
        return RedirectToAction("LogIn");
    }

}
