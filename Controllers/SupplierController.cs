using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp.SupplierVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class SupplierController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public SupplierController(ApplicationDbContext context, INotyfService notification)
    {
        _context = context;
        _notification = notification;
    }
    public async Task<IActionResult> Index(SupplierIndexVm vm)
    {
        vm.Suppliers = await _context.Suppliers.Where(x => (string.IsNullOrEmpty(vm.SearchString) || x.Name.Contains(vm.SearchString) || x.Address.Contains(vm.SearchString)) && vm.SupplierStatus == null || x.SupplierStatus == vm.SupplierStatus).ToListAsync();

        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new SupplierAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SupplierAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Data.");
            }

            var supplier = new Supplier();
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            supplier.Name = vm.Name;
            supplier.Address = vm.Address;
            supplier.PhoneNumber = vm.PhoneNumber;
            supplier.SupplierStatus = SupplierStatusConstants.Active;
            supplier.LastTransaction = null;

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            tx.Complete();
            _notification.Success("Supplier Added.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(long Id)
    {
        try
        {
            var supplier = await _context.Suppliers.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (supplier == null)
            {
                throw new Exception("No Supplier Found.");
            }
            var vm = new SupplierEditVm();
            vm.Name = supplier.Name;
            vm.Address = supplier.Address;
            vm.PhoneNumber = supplier.PhoneNumber;
            vm.SupplierStatus = supplier.SupplierStatus;

            return View(vm);
        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long Id, SupplierEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid data.");
            }

            var supplier = await _context.Suppliers.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (supplier == null)
            {
                throw new Exception("No Supplier Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            supplier.Name = vm.Name;
            supplier.Address = vm.Address;
            supplier.PhoneNumber = vm.PhoneNumber;
            supplier.SupplierStatus = vm.SupplierStatus;

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Supplier Edited.");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return View(vm);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var supplier = await _context.Suppliers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (supplier == null)
            {
                _notification.Warning("No Supplier Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Supplier Deleted.");
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return RedirectToAction("Index");
        }

    }
}
