using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Data;
using eKirana.Models;
using eKirana.ViewModels.PurchaseVms.PurchaseItemDetailVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Controllers
{
    public class PurchaseDetailController : Controller
    {
        private readonly INotyfService _notification;
        private readonly ApplicationDbContext _context;

        public PurchaseDetailController(ApplicationDbContext context, INotyfService notification)
        {
            _context = context;
            _notification = notification;
        }

        public async Task<IActionResult> Index(long PurchaseId, PurchaseDetailIndexVm vm)
        {
            var purchaseDetail = await _context.PurchaseDetails.Where(x => x.PurchaseId == PurchaseId && (vm.ProductId == null || x.ProductId == vm.ProductId)).Include(x => x.Product).Include(x => x.Unit).ToListAsync();

            vm.PurchaseDetailInfoVms = purchaseDetail.Select(x => new PurchaseDetailInfoVm()
            {
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                Rate = x.Rate,
                UnitName = x.Unit.Name,
                SubTotal = x.SubTotal,
                VATAmount = x.VATAmount,
                Discount = x.Discount,
                NetAmount = x.NetAmount
            }).ToList();

            //for searching
            var productPurchased = await _context.PurchaseDetails.Where(x => x.PurchaseId == PurchaseId).Include(x => x.Product).ToListAsync();

            vm.Products = productPurchased.Select(x => new Product()
            {
                Id = x.Product.Id,
                Name = x.Product.Name,
            }).ToList();
            return View(vm);
        }
    }
}