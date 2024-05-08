using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.ViewModels.StockQuantityVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class StockQuantityController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notificaion;
    public StockQuantityController(ApplicationDbContext context, INotyfService notificaion)
    {
        _context = context;
        _notificaion = notificaion;
    }
    public async Task<IActionResult> Index(StockQuantityIndexVm vm)
    {
        var stockQuantityHistrories = await _context.StockQuantityHistories.Where(x => (vm.ProductId == null || x.ProductQuantityUnitRate.ProductId == vm.ProductId) && (vm.AdminId == null || x.AdminId == vm.AdminId) && (vm.Remarks == null || x.Remarks == vm.Remarks)).Include(x => x.ProductQuantityUnitRate).ThenInclude(x => x.Product).Include(x => x.Admin).OrderByDescending(x => x.On_Date).ToListAsync();

        vm.StockQuantityInfoVms = stockQuantityHistrories.Select(x => new StockQuantityInfoVm()
        {
            ProductName = x.ProductQuantityUnitRate.Product.Name,
            ProductPhoto = x.ProductQuantityUnitRate.Product.Photo,
            QuantityMovement = x.QuantityMovement,
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
        var stockQuantity = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == ProductId).Include(x => x.Product).ThenInclude(x => x.Brand).ToListAsync();

        // var vm = new StockQuantityResetVm();
        // vm.ProductName = stockQuantity.Product.Name;
        // vm.ProductBrand = stockQuantity.Product.Brand.BrandName;
        // if (stockQuantity.IsBaseUnit)
        // {
        //     vm.StockQuantity = stockQuantity.Stock_Quantity;
        //     vm.UnitId = stockQuantity.UnitId;
        // }


        return View(vm);
    }
}
