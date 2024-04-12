using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models;
using eKirana.Provider.Interfaces;
using eKirana.ViewModels.PurchaseVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class PurchaseController : Controller
{
    private readonly INotyfService _notification;
    private readonly ApplicationDbContext _context;
    private readonly ICurrentAdminProvider _currentAdminProvider;
    public PurchaseController(INotyfService notyfService, ApplicationDbContext context, ICurrentAdminProvider currentAdminProvider)
    {
        _notification = notyfService;
        _context = context;
        _currentAdminProvider = currentAdminProvider;
    }

    public async Task<IActionResult> Index(PurchaseIndexVm vm)
    {
        vm.Purchases = await _context.Purchases.Where(x => (vm.SupplierId == null || vm.SupplierId == x.SupplierId) && (vm.PurchaseById == x.PurchaseById) && (vm.FromPurchaseDate == null || vm.FromPurchaseDate.Value.Date >= x.PurchaseDate.Date) && (vm.ToPurchaseDate == null || vm.ToPurchaseDate.Value.Date <= x.PurchaseDate.Date)).Include(x => x.Supplier).Include(x => x.Admin).ToListAsync();

        vm.Admins = await _context.Admins.ToListAsync();
        vm.Suppliers = await _context.Suppliers.ToListAsync();
        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new PurchaseFormAddVm();
        vm.Suppliers = await _context.Suppliers.ToListAsync();
        vm.Products = await _context.Products.ToListAsync();
        vm.Units = await _context.Units.ToListAsync();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PurchaseAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }

            var currentAdmin = await _currentAdminProvider.GetCurrentAdmin();

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            //changes on purchase model
            var purchase = new Purchase();
            purchase.SupplierId = vm.SupplierId;
            purchase.PurchaseDate = vm.PurchaseDate;
            purchase.PurchaseById = currentAdmin.Id;

            //changes on supplier model
            var supplier = await _context.Suppliers.Where(x => x.Id == vm.SupplierId).FirstOrDefaultAsync();
            if (supplier == null)
            {
                throw new Exception("No Supplier Found.");
            }
            supplier.LastTransaction = DateTime.Now;

            decimal? TotalAmount = 0.00m;

            foreach (var purchaseDetailVm in vm.PurchaseItems)
            {
                var purchaseDetail = new PurchaseDetail();
                purchaseDetail.PurchaseId = purchase.Id;
                purchaseDetail.ProductId = purchaseDetailVm.ProductId;
                purchaseDetail.Quantity = purchaseDetailVm.Quantity;
                purchaseDetail.UnitId = purchaseDetailVm.UnitId;
                purchaseDetail.Rate = purchaseDetailVm.Rate;
                purchaseDetail.SubTotal = purchaseDetailVm.SubTotal;
                purchaseDetail.VATAmount = purchaseDetailVm.VATAmount;
                purchaseDetail.Discount = purchaseDetailVm.Discount;
                purchaseDetail.NetAmount = purchaseDetailVm.NetAmount;

                var PrdQUR = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == purchaseDetailVm.ProductId).ToListAsync();
                foreach (var prdQUR in PrdQUR)
                {
                    if (prdQUR.UnitId == purchaseDetailVm.UnitId)
                    {
                        prdQUR.Stock_Quantity = prdQUR.Stock_Quantity + purchaseDetailVm.Quantity;
                    }
                    else
                    {
                        throw new Exception("Set and Add Valid Unit For Product.");
                    }
                    _context.ProductQuantityUnitRates.Update(prdQUR);

                }



                //for purchaseRate
                var purchaseRate = new ProductPurchaseRate();
                purchaseRate.ProductId = purchaseDetailVm.ProductId;
                purchaseRate.UnitId = purchaseDetailVm.UnitId;
                purchaseRate.Amount = purchaseDetailVm.Rate;
                purchaseRate.DateModified = DateTime.Now;

                TotalAmount = TotalAmount + purchaseDetail.NetAmount;

                _context.PurchaseDetails.Add(purchaseDetail);

                _context.ProductPurchaseRates.Add(purchaseRate);
            }

            purchase.TotalPaidAmount = TotalAmount;

            //_context.Suppliers.Update(supplier); not necessary
            _context.Purchases.Add(purchase);

            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Purchased Successfully.");
            return Json(new
            {
                success = true
            });
        }
        catch (Exception e)
        {
            _notification.Error(e.Message);
            return Json(new
            {
                success = false,
                error = e.Message

            });
        }
    }
    public async Task<IActionResult> GetProductRate(long? productId, long? unitId)
    {
        var PurchaseRate = await _context.ProductPurchaseRates.Where(x => x.ProductId == productId && x.UnitId == unitId).FirstOrDefaultAsync();


        if (PurchaseRate != null)
        {
            return Json(new
            {
                purchaseRate = PurchaseRate.Amount //purchaseRate
            });
        }

        else
        {
            return Json(new
            {
                error = "Product not found"
            });
        }

    }


}
