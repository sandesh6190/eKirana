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
        var prdQURs = await _context.ProductQuantityUnitRates.Where(x => (x.ProductId == ProductId) && (vm.UnitId == null || vm.UnitId == x.UnitId)).Include(x => x.Product).Include(x => x.Unit).OrderByDescending(x => x.Ratio).ToListAsync();

        var prdBaseStockQuantity = await _context.ProductQuantityUnitRates.Where(x => x.IsBaseUnit == true).FirstOrDefaultAsync();

        vm.InfoProductQuantityUnitRateVms = prdQURs.Select(x => new InfoProductQuantityUnitRateVm()
        {
            PrdQURId = x.Id,
            Product = x.Product,
            UnitId = x.Unit.Id,
            UnitName = x.Unit.Name,
            IsBaseUnit = x.IsBaseUnit,
            Ratio = x.Ratio,
        }).ToList();



        //filling remaining properties of InfoProductQuantityUnitRateVm from different models

        foreach (var prdQUR in vm.InfoProductQuantityUnitRateVms)
        {
            if (prdBaseStockQuantity.Stock_Quantity != 0)
            {
                prdQUR.Quantity = prdBaseStockQuantity.Stock_Quantity / prdQUR.Ratio;
                prdBaseStockQuantity.Stock_Quantity = prdBaseStockQuantity.Stock_Quantity % prdQUR.Ratio;
            }

            var purchaseRates = await _context.ProductPurchaseRates.Where(x => x.ProductId == ProductId && x.UnitId == prdQUR.UnitId).FirstOrDefaultAsync();
            if (purchaseRates != null)
            {
                prdQUR.PurchaseRate = purchaseRates.Amount;
            }

        }

        var saleRates = await _context.ProductSaleRates.Where(x => x.ProductId == ProductId).ToListAsync();

        foreach (var prdQUR in vm.InfoProductQuantityUnitRateVms)
        {
            foreach (var saleRate in saleRates)
            {
                if (prdQUR.UnitId == saleRate.UnitId)
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
                throw new Exception("Invalid Data."); //better technique
                // vm.ProductId = ProductId;
                // vm.Units = await _context.Units.ToListAsync();
                // _notification.Warning("Invalid Data.");
                // return View(vm);
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


    public async Task<IActionResult> Edit(long ProductId, long PrdQURId) //ProductId bharei Indexko lagi matrei ho
    {
        try
        {
            var prdQUR = await _context.ProductQuantityUnitRates.Where(x => x.Id == PrdQURId).FirstOrDefaultAsync();

            if (prdQUR == null)
            {
                throw new Exception("No Data Found.");
            }

            var prdPurchaseRate = await _context.ProductPurchaseRates.Where(x => x.ProductId == ProductId && x.UnitId == prdQUR.UnitId).FirstOrDefaultAsync();

            var prdSaleRate = await _context.ProductSaleRates.Where(x => x.ProductId == ProductId && x.UnitId == prdQUR.UnitId).FirstOrDefaultAsync();

            var vm = new EditProductQuantityUnitRateVm();
            vm.ProductId = ProductId; //for index page
            vm.Stock_Quantity = prdQUR.Stock_Quantity;
            vm.UnitId = prdQUR.UnitId;
            vm.IsBaseUnit = prdQUR.IsBaseUnit;
            vm.Ratio = prdQUR.Ratio;
            if (prdPurchaseRate == null)
            {
                vm.PurchaseRate = null;
            }
            else
            {
                vm.PurchaseRate = prdPurchaseRate.Amount;
            }

            if (prdSaleRate == null)
            {
                vm.SaleRate = null;
            }
            else
            {
                vm.SaleRate = prdSaleRate.Amount;
            }

            vm.Units = await _context.Units.ToListAsync();

            return View(vm);
        }

        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index", new { ProductId = ProductId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long ProductId, long PrdQURId, EditProductQuantityUnitRateVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }

            var prdQUR = await _context.ProductQuantityUnitRates.Where(x => x.Id == PrdQURId).FirstOrDefaultAsync();
            if (prdQUR == null)
            {
                throw new Exception("No Data Found.");
            }

            var prdPurchaseRate = await _context.ProductPurchaseRates.Where(x => x.ProductId == ProductId && x.UnitId == vm.UnitId).FirstOrDefaultAsync();

            var prdSaleRate = await _context.ProductSaleRates.Where(x => x.ProductId == ProductId && x.UnitId == vm.UnitId).FirstOrDefaultAsync();

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            prdQUR.UnitId = vm.UnitId;
            prdQUR.IsBaseUnit = vm.IsBaseUnit;
            prdQUR.Ratio = vm.Ratio;

            if (prdPurchaseRate != null)
            {
                prdPurchaseRate.Amount = vm.PurchaseRate;
                prdPurchaseRate.DateModified = DateTime.Now;
                _context.ProductPurchaseRates.Update(prdPurchaseRate);
            }
            else
            {
                var productPurchaseRate = new ProductPurchaseRate();
                productPurchaseRate.ProductId = ProductId;
                productPurchaseRate.UnitId = vm.UnitId;
                productPurchaseRate.Amount = vm.PurchaseRate;
                productPurchaseRate.DateModified = DateTime.Now;
                _context.ProductPurchaseRates.Add(productPurchaseRate);
            }

            if (prdSaleRate != null)
            {
                prdSaleRate.Amount = vm.SaleRate;
                prdSaleRate.DateModified = DateTime.Now;
                _context.ProductSaleRates.Update(prdSaleRate);
            }
            else
            {
                var productSaleRate = new ProductSaleRate();
                productSaleRate.ProductId = ProductId;
                productSaleRate.UnitId = vm.UnitId;
                productSaleRate.Amount = vm.SaleRate;
                productSaleRate.DateModified = DateTime.Now;
                _context.ProductSaleRates.Add(productSaleRate);
            }

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Record Edited.");
            return RedirectToAction("Index", new { ProductId = ProductId });
        }
        catch (Exception e)
        {
            vm.Units = await _context.Units.ToListAsync();
            _notification.Warning(e.Message);
            return View(vm);
        }
    }

    [HttpPost]
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

    public async Task<IActionResult> GetUnitSelectList(long ProductId)
    {
        var prdQURs = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId).Include(x => x.Unit).OrderBy(x => x.Ratio).ToListAsync();

        if (prdQURs != null)
        {
            return Json(new
            {
                prdQURs  //we can also send only those data which are needed.
            }
            );
        }
        else
        {
            return Json(new
            {
                error = "No Unit Found For Product So Set Unit For Product."
            });
        }
    }
}
