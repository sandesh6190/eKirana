using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class SeedingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    private readonly IAuthManager _authManager;
    public SeedingController(ApplicationDbContext context, INotyfService notification, IAuthManager authManager)
    {
        _context = context;
        _notification = notification;
        _authManager = authManager;
    }

    public async Task<IActionResult> SeedSuperAdmin()
    {
        try
        {
            var superAdmin = await _context.Admins.Where(x => x.AdminType == AdminTypeConstants.SuperAdmin).FirstOrDefaultAsync();
            if (superAdmin != null)
            {
                throw new Exception("Admin Already Existed.");
            }

            await _authManager.Register("Super Admin", "super.admin@gmail.com", "admin");
            _notification.Success("Super Admin Seeded Successfully.");
            return RedirectToAction("LogIn", "Authentication");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("LogIn", "Authentication");
        }
    }
}
