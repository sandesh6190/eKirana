using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
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
        vm.Purchases = await _context.Purchases.Where(x => (vm.SupplierId == null || vm.SupplierId == x.SupplierId) && (vm.PurchaseById == null || vm.PurchaseById == x.PurchaseById) && (vm.FromPurchaseDate == null || x.PurchaseDate.Date <= vm.FromPurchaseDate.Value.Date) && (vm.ToPurchaseDate == null || x.PurchaseDate.Date >= vm.ToPurchaseDate.Value.Date)).Include(x => x.Supplier).Include(x => x.Admin).ToListAsync();

        vm.Admins = await _context.Admins.Where(x => x.AdminStatus == AdminStatusConstants.Active).ToListAsync();
        vm.Suppliers = await _context.Suppliers.Where(x => x.SupplierStatus == SupplierStatusConstants.Active).ToListAsync();
        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new PurchaseFormAddVm();
        vm.Suppliers = await _context.Suppliers.Where(x => x.SupplierStatus == SupplierStatusConstants.Active).ToListAsync();
        vm.Products = await _context.Products.ToListAsync();
        //vm.Units = await _context.Units.ToListAsync(); fetching through api according to product

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

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();


            //changes on supplier model
            var supplier = await _context.Suppliers.Where(x => x.Id == vm.SupplierId).FirstOrDefaultAsync();
            if (supplier == null)
            {
                throw new Exception("No Supplier Found.");
            }
            supplier.LastTransaction = DateTime.Now;

            decimal TotalAmount = 0.00m;

            foreach (var purchaseDetailVm in vm.PurchaseItemVms)
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

                var prdQUR = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == purchaseDetailVm.ProductId && x.IsBaseUnit == true).FirstOrDefaultAsync();

                var prdRatio = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == purchaseDetailVm.ProductId && x.UnitId == purchaseDetailVm.UnitId).FirstOrDefaultAsync();

                if (prdQUR == null)
                {
                    throw new Exception("Add Base Unit of Product.");
                }
                if (prdRatio == null)
                {
                    throw new Exception("Add Ratio Of Product.");
                }

                //for StockQuantityHistory
                var stockQuantityHistory = new StockQuantityHistory();

                if (prdQUR.UnitId != purchaseDetailVm.UnitId)
                {
                    long baseStockQuantity = purchaseDetailVm.Quantity * prdRatio.Ratio;
                    prdQUR.Stock_Quantity = prdQUR.Stock_Quantity + baseStockQuantity;


                    stockQuantityHistory.QuantityMovement = baseStockQuantity;
                }
                else
                {
                    prdQUR.Stock_Quantity = prdQUR.Stock_Quantity + purchaseDetailVm.Quantity;
                    stockQuantityHistory.QuantityMovement = purchaseDetailVm.Quantity;

                }

                stockQuantityHistory.ProductQuantityUnitRateId = prdQUR.Id;
                stockQuantityHistory.Remarks = StockQuantityRemarksConstants.Purchased;
                stockQuantityHistory.On_Date = DateTime.Now;
                stockQuantityHistory.AdminId = currentAdmin.Id;


                _context.StockQuantityHistories.Add(stockQuantityHistory);
                _context.ProductQuantityUnitRates.Update(prdQUR);

                //for purchaseRate
                var purchaseRate = await _context.ProductPurchaseRates.Where(x => x.UnitId == purchaseDetailVm.UnitId && x.ProductId == purchaseDetailVm.ProductId).FirstOrDefaultAsync();

                if (purchaseRate == null)
                {
                    var purRate = new ProductPurchaseRate();
                    purRate.ProductId = purchaseDetailVm.ProductId;
                    purRate.UnitId = purchaseDetailVm.UnitId;
                    purRate.Amount = purchaseDetailVm.Rate;
                    purRate.DateModified = DateTime.Now;
                    _context.ProductPurchaseRates.Add(purRate);
                }
                else if (purchaseRate != null)
                {
                    purchaseRate.Amount = purchaseDetailVm.Rate;
                    purchaseRate.DateModified = DateTime.Now;
                    _context.ProductPurchaseRates.Update(purchaseRate);
                }
                //end of productpuchaserate}

                TotalAmount = TotalAmount + purchaseDetail.NetAmount;

                _context.PurchaseDetails.Add(purchaseDetail);

            }

            var purchaseFromDB = await _context.Purchases.FirstOrDefaultAsync(x => x.Id == purchase.Id);
            purchaseFromDB.TotalPaidAmount = TotalAmount;

            //_context.Suppliers.Update(supplier); not necessary
            _context.Purchases.Update(purchaseFromDB);

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
    public async Task<IActionResult> GetProductPurchaseRate(long? productId, long? unitId)
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
                error = "Product Purchase Rate not found"
            });
        }

    }

}
