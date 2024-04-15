using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms.PurchaseItemDetailVms;
public class PurchaseItemVm
{
    public long ProductId { get; set; }
    public decimal Rate { get; set; }
    public long Quantity { get; set; }
    //public long BaseQuantity { get; set; }
    public long UnitId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal VATAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal NetAmount { get; set; }

}
