using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eKirana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms.PurchaseItemDetailVms
{
    public class PurchaseDetailIndexVm
    {
        public long? PurchaseId { get; set; }
        public long? ProductId { get; set; }
        public List<Product> Products { get; set; }
        public SelectList ProductSelectList()
        {
            return new SelectList(
                Products,
                nameof(Product.Id),
                nameof(Product.Name),
                ProductId
            );
        }

        public List<PurchaseDetailInfoVm> PurchaseDetailInfoVms;
    }
}