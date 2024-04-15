using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.ViewModels.SaleVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class SaleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public SaleController(ApplicationDbContext context, INotyfService notification)
    {
        _context = context;
        _notification = notification;
    }

    public async Task<IActionResult> Index(SaleIndexVm vm)
    {
        var Sales = await _context.Sales.Where(x => string.IsNullOrEmpty(vm.SearchCustomer) || x.CustomerName.Contains(vm.SearchCustomer) || x.CustomerName.Contains(vm.SearchCustomer)).Include(x => x.Admin).ToListAsync();

        vm.SaleInfoVms = Sales.Select(x => new SaleInfoVm()
        {
            SaleId = x.Id,
            CustomerName = x.CustomerName,
            CustomerAddress = x.CustomerAddress,
            SaleDate = DateTime.Now,
            SaleBy = x.Admin.UserName,
            TotalAmount = x.TotalAmount,
        }).ToList();

        //we can add more data on vm through different model

        vm.Admins = await _context.Admins.Where(x => x.AdminStatus == AdminStatusConstants.Active).ToListAsync();
        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new SaleFormAddVm();
        vm.Products = await _context.Products.ToListAsync();
        vm.Units = await _context.Units.ToListAsync();

        return View(vm);
    }
}
