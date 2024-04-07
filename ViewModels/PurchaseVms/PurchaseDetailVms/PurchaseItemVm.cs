using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms.PurchaseDetailVms;
public class PurchaseItemVm
{
    public long ProductId { get; set; }
    // public List<Product> Products;
    // public SelectList ProductSelectList()
    // {
    //     return new SelectList(
    //         Products,
    //         nameof(Product.Id),
    //         nameof(Product.Name),
    //         ProductId
    //     );
    // }
    public decimal Rate { get; set; }
    public decimal Quantity { get; set; }
    public long UnitId { get; set; }
    // public List<Unit> Units;
    // public SelectList UnitSelectList()
    // {
    //     return new SelectList(
    //     Units,
    //     nameof(Unit.Id),
    //     nameof(Unit.Name),
    //     UnitId
    //     );
    // }
    public decimal SubTotal { get; set; }
    // public decimal TotalAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal NetAmount { get; set; }

}
