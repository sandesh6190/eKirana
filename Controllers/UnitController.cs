using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp.UnitVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class UnitController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public UnitController(ApplicationDbContext context, INotyfService notyfService)
    {
        _context = context;
        _notification = notyfService;
    }

    public async Task<IActionResult> Index(UnitIndexVm vm)
    {
        vm.Units = await _context.Units.Where(x => string.IsNullOrEmpty(vm.Name) || x.Name.Contains(vm.Name)).ToListAsync();

        return View(vm);
    }

    public IActionResult Add()
    {
        var vm = new UnitAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(UnitAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _notification.Error("Invalid Data.");
                return View(vm);
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var unit = new Unit();
            unit.Name = vm.Name;
            unit.DateModified = DateTime.Now;

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Unit Added.");

            return RedirectToAction("Index");

        }

        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            var unit = await _context.Units.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (unit == null)
            {
                throw new Exception("No Unit Found.");
            }


            var vm = new UnitEditVm();
            vm.Name = unit.Name;

            return View(vm);
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, UnitEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _notification.Error("Invalid Data.");
                return View(vm);
            }

            var unit = await _context.Units.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (unit == null)
            {
                throw new Exception("No Unit Found.");
            }
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            unit.Name = vm.Name;
            unit.DateModified = DateTime.Now;

            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Unit Edited.");

            return RedirectToAction("Index");

        }

        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var unit = await _context.Units.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (unit == null)
            {
                throw new Exception("No Unit Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Unit Deleted.");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }
}
