using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp.CategoryVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public CategoryController(ApplicationDbContext context, INotyfService notyfService)
    {
        _context = context;
        _notification = notyfService;
    }
    public async Task<IActionResult> Index(CategoryIndexVm vm)
    {
        vm.Categories = await _context.Categories.Where(x => string.IsNullOrEmpty(vm.Item) || x.Item.Contains(vm.Item)).ToListAsync();
        return View(vm);
    }

    public IActionResult Add()
    {
        var vm = new CategoryAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CategoryAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Data Input.");
            }

            var cat = await _context.Categories.Where(x => x.Item == vm.Item).FirstOrDefaultAsync();
            if (cat != null)
            {
                throw new Exception("Category Already Exist.");
            }

            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var category = new Category();
                category.Item = vm.Item;
                category.DateModified = DateTime.Now;

                //mark an object to be inserted
                _context.Categories.Add(category);

                //send data to database
                await _context.SaveChangesAsync();

                //transaction complete
                tx.Complete();
            }
            _notification.Success("Category Added.");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            var category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new Exception("Category Couldn't Found.");
            }

            var vm = new CategoryEditVm();
            vm.Item = category.Item;

            return View(vm);

        }
        catch (Exception e)
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, CategoryEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Data.");
            }
            var cat = await _context.Categories.Where(x => x.Item == vm.Item).FirstOrDefaultAsync();
            if (cat != null)
            {
                throw new Exception("Category Already Exist.");
            }

            var category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                throw new Exception("Category Name Existed.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            category.Item = vm.Item;
            category.DateModified = DateTime.Now;

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Category Edited.");
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
            var category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new Exception("Category Couldn't Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Item Deleted.");

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }

    }

}
