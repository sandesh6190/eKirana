using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Models;
using eKirana.Models.SetUp;
using eKirana.Provider;
using eKirana.Provider.Interfaces;
using eKirana.ViewModels.SaleVms;
using eKirana.ViewModels.SetUp.MemberShipVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers;
public class SaleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    private readonly ICurrentAdminProvider _currentAdminProvider;
    public SaleController(ApplicationDbContext context, INotyfService notification, ICurrentAdminProvider currentAdminProvider)
    {
        _context = context;
        _notification = notification;
        _currentAdminProvider = currentAdminProvider;
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
        //sending all the members' data using separate vm instead of using model itself
        var members = await _context.MemberShipHolders.ToListAsync();

        vm.MemberShipInfoVms = members.Select(x => new MemberShipInfoVm()
        {
            MemberShipId = x.Id,
            Name = x.Name,
            Address = x.Address,
            Phone = x.Phone,
            LastTransaction = x.LastTransaction
        }).ToList();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] SaleAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }

            var currentAdmin = await _currentAdminProvider.GetCurrentAdmin();

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var sale = new Sale();
            sale.SaleDate = vm.SaleDate;
            sale.SaleById = currentAdmin.Id;

            if (vm.CustomerType == CustomerTypeConstants.NormalCustomer)
            {
                sale.CustomerType = vm.CustomerType;
                sale.CustomerName = vm.CustomerName;
                sale.CustomerAddress = vm.CustomerAddress;
            }
            else if (vm.CustomerType == CustomerTypeConstants.MemberShipCustomer)
            {
                sale.CustomerType = vm.CustomerType;
                sale.MemberShipId = vm.MemberShipId;
                var member = await _context.MemberShipHolders.Where(x => x.Id == vm.MemberShipId).FirstOrDefaultAsync();
                if (member == null)
                {
                    throw new Exception("No Member Found.");
                }
                member.LastTransaction = DateTime.Now;

                _context.MemberShipHolders.Update(member);
            }

            decimal TotalAmount = 0;

            foreach (var saleItemDetail in vm.SaleItemVms)
            {
                var saleDetail = new SaleDetail();
                saleDetail.SaleId = sale.Id;
                saleDetail.ProductId = saleItemDetail.ProductId;
                saleDetail.Quantity = saleItemDetail.Quantity;
                saleDetail.UnitId = saleItemDetail.UnitId;
                saleDetail.Rate = saleItemDetail.Rate;
                saleDetail.SubTotal = saleItemDetail.SubTotal;
                saleDetail.VATAmount = saleItemDetail.VATAmt;
                saleDetail.Discount = saleItemDetail.DisAmt;
                saleDetail.NetAmount = saleItemDetail.NetAmt;

                //for Stock Quantity{
                var prdQUR = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == saleItemDetail.ProductId && x.IsBaseUnit == true).FirstOrDefaultAsync();

                var prdRatio = await _context.ProductQuantityUnitRates.Where(x => x.ProductId == saleItemDetail.ProductId && x.UnitId == saleItemDetail.UnitId).FirstOrDefaultAsync();

                if (prdQUR == null || prdRatio == null)
                {
                    throw new Exception("Exception occured.");
                }

                //for StockQuantityHistory{
                var stockQuantityHistory = new StockQuantityHistory();

                if (prdQUR.UnitId != saleDetail.UnitId)
                {
                    long baseStockQuantity = saleDetail.Quantity * prdRatio.Ratio;
                    prdQUR.Stock_Quantity = prdQUR.Stock_Quantity + baseStockQuantity;

                    stockQuantityHistory.QuantityMovement = baseStockQuantity;
                }

                else
                {
                    prdQUR.Stock_Quantity = prdQUR.Stock_Quantity + saleDetail.Quantity;
                    stockQuantityHistory.QuantityMovement = saleDetail.Quantity;
                }

                stockQuantityHistory.ProductQuantityUnitRateId = prdQUR.Id;
                stockQuantityHistory.Remarks = StockQuantityRemarksConstants.Sold;
                stockQuantityHistory.On_Date = DateTime.Now;
                _context.StockQuantityHistories.Add(stockQuantityHistory);
                //end for StockQuantityHistory}
                _context.ProductQuantityUnitRates.Update(prdQUR);
                //end for Stock Quantity}

                TotalAmount = TotalAmount + saleItemDetail.NetAmt;
                _context.SaleDetails.Add(saleDetail);
                //end of saleDetail}
            }

            sale.TotalAmount = TotalAmount;
            _context.Sales.Add(sale);
            //end of sale}

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Sold Successfully.");
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

    public async Task<IActionResult> GetProductSaleRate(long ProductId, long UnitId)
    {
        var prdSaleRate = await _context.ProductSaleRates.Where(x => x.ProductId == ProductId && x.UnitId == UnitId).FirstOrDefaultAsync();

        if (prdSaleRate != null)
        {
            return Json(new
            {
                saleRate = prdSaleRate.Amount
            });
        }
        else
        {
            return Json(new
            {
                error = "No Sale Rate is assign to product"
            });
        }
    }
}
