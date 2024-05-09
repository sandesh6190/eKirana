using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Models;
using eKirana.Models.SetUp;
using eKirana.Provider.Interfaces;
using eKirana.ViewModels.StockQuantityVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class StockQuantityController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    private readonly ICurrentAdminProvider _currentAdminProvider;
    public StockQuantityController(ApplicationDbContext context, INotyfService notification, ICurrentAdminProvider currentAdminProvider)
    {
        _context = context;
        _notification = notification;
        _currentAdminProvider = currentAdminProvider;
    }
    public async Task<IActionResult> Index(StockQuantityIndexVm vm)
    {
        var stockQuantityHistrories = await _context.StockQuantityHistories.Where(x => (vm.ProductId == null || x.ProductQuantityUnitRate.ProductId == vm.ProductId) && (vm.AdminId == null || x.AdminId == vm.AdminId) && (vm.Remarks == null || x.Remarks == vm.Remarks)).Include(x => x.ProductQuantityUnitRate).ThenInclude(x => x.Product).Include(x => x.Admin).Include(x => x.Unit).OrderByDescending(x => x.On_Date).ToListAsync();

        vm.StockQuantityInfoVms = stockQuantityHistrories.Select(x => new StockQuantityInfoVm()
        {
            ProductName = x.ProductQuantityUnitRate.Product.Name,
            ProductPhoto = x.ProductQuantityUnitRate.Product.Photo,
            QuantityMovement = x.QuantityMovement,
            UnitName = x.Unit.Name,
            AdminName = x.Admin.UserName,
            On_Date = x.On_Date,
            Remarks = x.Remarks,
        }).ToList();

        vm.Products = await _context.Products.ToListAsync();
        vm.Admins = await _context.Admins.ToListAsync();

        return View(vm);
    }

    public async Task<IActionResult> Reset(long ProductId)
    {
        var stockQuantity = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId && x.IsBaseUnit == true).Include(x => x.Product).ThenInclude(x => x.Brand).FirstOrDefaultAsync();

        var vm = new StockQuantityResetVm();
        vm.ProductId = ProductId;
        vm.ProductName = stockQuantity.Product.Name;
        vm.ProductBrand = stockQuantity.Product.Brand.BrandName;
        vm.StockQuantity = stockQuantity.Stock_Quantity;
        vm.UnitId = stockQuantity.UnitId;

        var productUnits = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId).Include(x => x.Unit).ToListAsync();

        vm.Units = productUnits.Select(x => new Unit()
        {
            Id = x.Unit.Id,
            Name = x.Unit.Name,
        }).ToList();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Reset(long ProductId, StockQuantityResetVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }
            var productStockQuantity = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId && x.IsBaseUnit == true).FirstOrDefaultAsync();
            if (productStockQuantity == null)
            {
                throw new Exception("No Data Found.");
            }

            var prdRatio = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId && x.UnitId == vm.UnitId).FirstOrDefaultAsync();
            if (prdRatio == null)
            {
                throw new Exception("No Unit Found.");
            }

            var currentAdmin = await _currentAdminProvider.GetCurrentAdmin();

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var stockQuantityHistory = new StockQuantityHistory();
            if (productStockQuantity.UnitId == vm.UnitId)
            {
                stockQuantityHistory.QuantityMovement = vm.StockQuantity;

                productStockQuantity.Stock_Quantity = vm.StockQuantity;

            }
            else
            {
                long? baseStockQuantity = vm.StockQuantity * prdRatio.Ratio;
                productStockQuantity.Stock_Quantity = baseStockQuantity;

                stockQuantityHistory.QuantityMovement = vm.StockQuantity;

            }
            stockQuantityHistory.ProductQuantityUnitRateId = productStockQuantity.Id;
            stockQuantityHistory.UnitId = vm.UnitId;
            stockQuantityHistory.Remarks = StockQuantityRemarksConstants.Reset;
            stockQuantityHistory.On_Date = DateTime.Now;
            stockQuantityHistory.AdminId = currentAdmin.Id;

            _context.ProductQuantityUnitRates.Update(productStockQuantity);
            _context.StockQuantityHistories.Add(stockQuantityHistory);

            await _context.SaveChangesAsync();
            tx.Complete();
            _notification.Success("Product Stock Quantity Reset Successfully.");
            return RedirectToAction("Index", "ProductQuantityUnitRate", new { ProductId = ProductId });
        }

        catch (Exception e)
        {
            _notification.Error(e.Message);

            var productUnits = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId).Include(x => x.Unit).ToListAsync();

            vm.Units = productUnits.Select(x => new Unit()
            {
                Id = x.Unit.Id,
                Name = x.Unit.Name,
            }).ToList();

            return View(vm);
        }
    }
}
