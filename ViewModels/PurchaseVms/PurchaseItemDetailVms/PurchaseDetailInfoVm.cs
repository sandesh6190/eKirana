using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//duplication of PurchaseItemVm
namespace eKirana.ViewModels.PurchaseVms.PurchaseItemDetailVms
{
    public class PurchaseDetailInfoVm
    {
        public string ProductName { get; set; }
        public decimal Rate { get; set; }
        public long Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
    }
}