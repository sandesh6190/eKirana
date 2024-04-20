using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models;
using eKirana.ViewModels.ProductVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eKirana.Controllers;
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, INotyfService notyfService)
    {
        _context = context;
        _notification = notyfService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index(ProductIndexVm vm)
    {
        //mapping with new list of vm

        //this is the how we add data on customize vm 
        // var productsWithRate = await _context.Products.Where(x => (string.IsNullOrEmpty(vm.Search)) || x.Name.Contains(vm.Search)
        //  && (vm.ProductVATorNOT == null || vm.ProductVATorNOT == x.ProductVATorNOT)).Include(x => x.Category).ToListAsync();

        // vm.ProductWithPurchaseRateVms = productsWithRate.Select(x => new ProductWithPurchaseRateVm()
        // {
        //     ProductId = x.Id,
        //     Name = x.Name,
        //     Photo = x.Photo,
        //     ProductVATorNOT = x.ProductVATorNOT,
        //     Category = x.Category
        //     // PurchaseRateAmt = vm.ProductPurchaseRates.Amount;

        // }).ToList();


        // vm.ProductPurchaseRates = await _context.ProductPurchaseRates.ToListAsync();
        //this is how we add extra property on list of vm
        // foreach (var product in vm.ProductWithPurchaseRateVms)
        // {
        //     foreach (var productRate in vm.ProductPurchaseRates)
        //     {
        //         if (product.ProductId == productRate.ProductId)
        //         {
        //             product.PurchaseRateAmt = productRate.Amount;
        //         }
        //     }
        // }
        var products = await _context.Products.Where(x =>
        (string.IsNullOrEmpty(vm.Search) || x.Name.Contains(vm.Search))
        && (vm.BrandId == null || x.BrandId == vm.BrandId)
        && (vm.CategoryId == null || vm.CategoryId == x.CategoryId)
        && (vm.ProductVATorNOT == null || vm.ProductVATorNOT == x.ProductVATorNOT)).Include(x => x.Category).Include(x => x.Brand).ToListAsync();

        //this is how we store data on vm
        vm.ProductInfoVms = products.Select(x => new ProductInfoVm()
        {
            ProductId = x.Id,
            Name = x.Name,
            Photo = x.Photo,
            BrandName = x.Brand.BrandName,
            ProductVATorNOT = x.ProductVATorNOT,
            Category = x.Category //not optimized technique, still done to understand
        }).ToList();

        //if we have extra property on vm we can add here using either foreach loop or others as above, also done on ProductQuantityUnitRateController

        //for searching process
        vm.Brands = await _context.Brands.ToListAsync();
        vm.Categories = await _context.Categories.ToListAsync();


        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new ProductAddVm();
        vm.Brands = await _context.Brands.ToListAsync();
        vm.Categories = await _context.Categories.ToListAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }

            var prd = await _context.Products.Where(x => x.Name == vm.Name && x.Brand.Id == vm.BrandId).FirstOrDefaultAsync();

            if (prd != null)
            {
                _notification.Warning("Product With Given Brand Already Existed.");
                return View(vm);
            }

            if (vm.Photo == null)
            {
                throw new Exception("No Photo Selected.");

            }

            //for Photo
            //for getting extension of a file
            var extension = Path.GetExtension(vm.Photo?.FileName);
            //getting random fileName
            var fileName = Guid.NewGuid().ToString() + "." + extension;
            //setting root directory for uploadig file
            var rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Photos");
            //setting file path
            var filePath = Path.Combine(rootPath, fileName);
            //FileStream is necessary for it,so creating Variable of FileStream with filePath and copying vm.Poster into stream
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.Photo?.CopyToAsync(stream);
            };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var product = new Product();
            product.Name = vm.Name;
            product.Photo = fileName;
            product.BrandId = vm.BrandId;
            product.CategoryId = vm.CategoryId;
            product.ProductVATorNOT = vm.ProductVATorNOT;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            tx.Complete();

            _notification.Success("Product Added.");
            return RedirectToAction("Index");


        }
        catch (Exception e)
        {
            vm.Brands = await _context.Brands.ToListAsync();
            vm.Categories = await _context.Categories.ToListAsync();
            _notification.Error(e.Message);
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(long Id)
    {
        try
        {
            var prd = await _context.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (prd == null)
            {
                throw new Exception("Product Doesn't Exist.");
            }
            var vm = new ProductEditVm();
            vm.Name = prd.Name;
            // vm.Photo = prd.Photo;
            vm.BrandId = prd.BrandId;
            vm.CategoryId = prd.CategoryId;
            vm.ProductVATorNOT = prd.ProductVATorNOT;

            vm.Categories = await _context.Categories.ToListAsync();
            vm.Brands = await _context.Brands.ToListAsync();

            return View(vm);

        }

        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long Id, ProductEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // _notification.Warning("Invalid Input.");
                // vm.Categories = await _context.Categories.ToListAsync();
                // return View(vm);
                throw new Exception("Invalid Input.");
            }

            var product = await _context.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new Exception("No Product Available.");
            }

            string? photoFileName = null;
            if (vm.Photo != null)
            {
                //getting extension of a file
                var extension = Path.GetExtension(vm.Photo.FileName);
                //getting random filename
                photoFileName = Guid.NewGuid().ToString() + "." + extension;
                //setting root directory for uploading file
                var rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Photos");
                //setting file path
                var filePath = Path.Combine(rootPath, photoFileName);

                //FileStream is necessary for it,so creating Variable of FileStream with filePath and copying vm.Poster into stream
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.Photo.CopyToAsync(stream);
                };

            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            product.Name = vm.Name;
            product.BrandId = vm.BrandId;
            product.CategoryId = vm.CategoryId;
            product.ProductVATorNOT = vm.ProductVATorNOT;

            if (!String.IsNullOrEmpty(photoFileName))
            {
                product.Photo = photoFileName;
            }

            await _context.SaveChangesAsync();
            tx.Complete();
            _notification.Success("Product Edited.");

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            vm.Categories = await _context.Categories.ToListAsync();
            _notification.Error(e.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long Id)
    {
        try
        {
            var product = await _context.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new Exception("No Product Available.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Product Deleted.");
            return RedirectToAction("Index");
        }

        catch (Exception e)
        {
            _notification.Error(e.Message);
            return RedirectToAction("Index");
        }
    }
}
