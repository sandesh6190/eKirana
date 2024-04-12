
using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp;
using eKirana.ViewModels.SetUp.BrandVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class BrandController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public BrandController(ApplicationDbContext context, INotyfService notyfService)
    {
        _context = context;
        _notification = notyfService;
    }
    public async Task<IActionResult> Index(BrandIndexVm vm)
    {
        vm.Brands = await _context.Brands.Where(x => string.IsNullOrEmpty(vm.BrandSearch) || x.BrandName.Contains(vm.BrandSearch)).ToListAsync();
        return View(vm);
    }

    public IActionResult Add()
    {
        var vm = new BrandAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(BrandAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _notification.Warning("Invalid Data Input.");
                return View(vm);
            }

            var brd = await _context.Brands.Where(x => x.BrandName == vm.BrandName).FirstOrDefaultAsync();
            if (brd != null)
            {
                _notification.Warning("Brand Already Exist.");
                return View(vm);
            }

            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var brand = new Brand();
                brand.BrandName = vm.BrandName;
                brand.DateModified = DateTime.Now;

                //mark an object to be inserted
                _context.Brands.Add(brand);

                //send data to database
                await _context.SaveChangesAsync();

                //transaction complete
                tx.Complete();
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            var brand = await _context.Brands.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (brand == null)
            {
                throw new Exception("Brand Couldn't Found.");
            }

            var vm = new BrandEditVm();
            vm.BrandName = brand.BrandName;

            return View(vm);

        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, BrandEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var brand = await _context.Brands.Where(x => x.Id == id).FirstOrDefaultAsync();

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            brand.BrandName = vm.BrandName;
            brand.DateModified = DateTime.Now;

            await _context.SaveChangesAsync();
            tx.Complete();

            return RedirectToAction("Index");
        }

        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var brand = await _context.Brands.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (brand == null)
            {
                throw new Exception("Brand Couldn't Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("BrandName Deleted.");

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }

    }

}
