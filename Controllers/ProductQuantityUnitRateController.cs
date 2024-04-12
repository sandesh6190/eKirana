using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using eKirana.Data;
using eKirana.Models;
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

    [HttpGet]
    public async Task<IActionResult> Index(long ProductId, IndexProductQuantityUnitRateVm vm)
    {
        vm.ProductId = ProductId; //passing product btween different view pages of different controller
        vm.Units = await _context.Units.ToListAsync();
        var prdQURs = await _context.ProductQuantityUnitRates.Where(x => (x.ProductId == ProductId) && (vm.UnitId == null || vm.UnitId == x.UnitId)).Include(x => x.Product).Include(x => x.Unit).ToListAsync();

        vm.InfoProductQuantityUnitRateVms = prdQURs.Select(x => new InfoProductQuantityUnitRateVm()
        {
            PrdQURId = x.Id,
            Product = x.Product,
            Quantity = x.Stock_Quantity,
            Unit = x.Unit,
            IsBaseUnit = x.IsBaseUnit,
            Ratio = x.Ratio,
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
    public async Task<IActionResult> AddProductUnit(long ProductId)
    {
        var vm = new AddProductQuantityUnitRateVm();
        vm.ProductId = ProductId;
        vm.Units = await _context.Units.ToListAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductUnit(long ProductId, AddProductQuantityUnitRateVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                vm.ProductId = ProductId;
                vm.Units = await _context.Units.ToListAsync();
                _notification.Warning("Invalid Data.");
                return View(vm);
            }

            var prdUnit = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId && x.UnitId == vm.UnitId).FirstOrDefaultAsync();
            if (prdUnit != null)
            {
                vm.ProductId = ProductId;
                vm.Units = await _context.Units.ToListAsync();
                _notification.Warning("Unit Already Exist For Product.");
                return View(vm);
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var prdQUR = new ProductQuantityUnitRate();
            prdQUR.ProductId = ProductId;
            prdQUR.UnitId = vm.UnitId;
            prdQUR.IsBaseUnit = vm.IsBaseUnit;
            prdQUR.Ratio = vm.Ratio;
            prdQUR.Stock_Quantity = 0;

            _context.ProductQuantityUnitRates.Add(prdQUR);
            await _context.SaveChangesAsync();

            tx.Complete();

            _notification.Success("Unit For Product Added.");

            return RedirectToAction("Index", new { ProductId = ProductId });
        }
        catch (Exception e)
        {
            vm.ProductId = ProductId;
            vm.Units = await _context.Units.ToListAsync();
            _notification.Error(e.Message);
            return View(vm);
        }
    }

    public async Task<IActionResult> Delete(long ProductId, long prdQURId) //ProductId is for passing ProductId to index action and page
    {
        try
        {
            var prdQUR = await _context.ProductQuantityUnitRates.Where(x => x.Id == prdQURId).FirstOrDefaultAsync();
            if (prdQUR == null)
            {
                throw new Exception("Row Couldn't Found");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.ProductQuantityUnitRates.Remove(prdQUR);
            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Unit For Product Deleted.");
            return RedirectToAction("Index", new { ProductId = ProductId });
        }
        catch (Exception e)
        {
            _notification.Warning(e.Message);
            return RedirectToAction("Index", new { ProductId = ProductId });
        }
    }
}
