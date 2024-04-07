using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models;
using eKirana.ViewModels.PurchaseVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class PurchaseController : Controller
{
    private readonly INotyfService _notification;
    private readonly ApplicationDbContext _context;
    public PurchaseController(INotyfService notyfService, ApplicationDbContext context)
    {
        _notification = notyfService;
        _context = context;
    }

    public async Task<IActionResult> Index(PurchaseIndexVm vm)
    {
        vm.Purchases = await _context.Purchases.Where(x => (vm.SupplierId == null || vm.SupplierId == x.SupplierId) && (vm.PurchaseById == x.PurchaseById) && (vm.FromPurchaseDate == null || vm.FromPurchaseDate.Value.Date >= x.PurchaseDate.Date) && (vm.ToPurchaseDate == null || vm.ToPurchaseDate.Value.Date <= x.PurchaseDate.Date)).Include(x => x.Supplier).Include(x => x.Admin).ToListAsync();

        vm.Admins = await _context.Admins.ToListAsync();
        vm.Suppliers = await _context.Suppliers.ToListAsync();
        return View(vm);
    }

    public async Task<IActionResult> Add(PurchaseFormAddVm vm)
    {
        vm.Suppliers = await _context.Suppliers.ToListAsync();
        vm.Products = await _context.Products.ToListAsync();
        vm.Units = await _context.Units.ToListAsync();

        return View(vm);
    }
    public async Task<IActionResult> GetProductRate(long? productId, long? unitId)
    {
        var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();
        var unit = await _context.Units.Where(x => x.Id == unitId).FirstOrDefaultAsync();

        if (product != null)
        {
            if (unit != null)
            {
                return Json(new
                {
                    purchaseRate = product.ProductPurchaseRate.Amount //purchaseRate
                });
            }
            else
            {
                return Json(new
                {
                    error = "No unit selected"
                });
            }
        }
        else
        {
            return Json(new
            {
                error = "Product not found"
            });
        }

    }
}
