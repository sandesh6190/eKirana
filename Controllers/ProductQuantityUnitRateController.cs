using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using eKirana.Data;
using eKirana.ViewModels.ProductQuantityUnitRateVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eKirana.Controllers;
public class ProductQuantityUnitRateController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public ProductQuantityUnitRateController(ApplicationDbContext context, INotyfService notification)
    {
        _context = context;
        _notification = notification;
    }

    public async Task<IActionResult> Index(long? ProductId, IndexProductQuantityUnitRateVm vm)
    {
        var prdQURs = await _context.ProductQuantityUnitRates.Where(x => (x.ProductId == ProductId) && (vm.UnitId == null || vm.UnitId == x.UnitId)).Include(x => x.Product).Include(x => x.Unit).ToListAsync();

        vm.InfoProductQuantityUnitRateVms = prdQURs.Select(x => new InfoProductQuantityUnitRateVm()
        {
            Product = x.Product,
            Quanity = x.Stock_Quantity,
            Unit = x.Unit,

        }).ToList();

        //filling remaining properties of InfoProductQuantityUnitRateVm from different models

        var purchaseRates = await _context.ProductPurchaseRates.Where(x => x.ProductId == ProductId).ToListAsync();

        foreach (var prdQUR in vm.InfoProductQuantityUnitRateVms)
        {
            foreach (var purchaseRate in purchaseRates)
            {
                if (prdQUR.Unit.Id == purchaseRate.UnitId)
                {
                    prdQUR.PurchaseRate = purchaseRate.Amount;
                }
            }
        }

        var saleRates = await _context.ProductSaleRates.Where(x => x.ProductId == ProductId).ToListAsync();

        foreach (var prdQUR in vm.InfoProductQuantityUnitRateVms)
        {
            foreach (var saleRate in saleRates)
            {
                if (prdQUR.Unit.Id == saleRate.UnitId)
                {
                    prdQUR.SaleRate = saleRate.Amount;
                }
            }
        }
        return View(vm);
    }

}
