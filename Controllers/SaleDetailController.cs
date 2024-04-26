using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models;
using eKirana.ViewModels.SaleVms.SaleItemDetailVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class SaleDetailController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public SaleDetailController(ApplicationDbContext context, INotyfService notification)
    {
        _context = context;
        _notification = notification;
    }

    public async Task<IActionResult> Index(long SaleId, SaleDetailIndexVm vm)
    {
        var soldProductDetails = await _context.SaleDetails.Where(x => x.SaleId == SaleId && (vm.ProductId == null || x.ProductId == vm.ProductId)).Include(x => x.Product).Include(x => x.Unit).ToListAsync();

        vm.SaleDetailInfoVms = soldProductDetails.Select(x => new SaleDetailInfoVm
        {
            SaleId = x.SaleId,
            ProductName = x.Product.Name,
            Quantity = x.Quantity,
            UnitName = x.Unit.Name,
            Rate = x.Rate,
            SubTotal = x.SubTotal,
            VATAmt = x.VATAmount,
            DisAmt = x.Discount,
            NetAmt = x.NetAmount,
        }).ToList();

        var soldProducts = await _context.SaleDetails.Where(x => x.SaleId == SaleId).Include(x => x.Product).ToListAsync();

        vm.Products = soldProducts.Select(x => new Product
        {
            Id = x.Product.Id,
            Name = x.Product.Name,
        }).ToList();

        return View(vm);
    }
}
