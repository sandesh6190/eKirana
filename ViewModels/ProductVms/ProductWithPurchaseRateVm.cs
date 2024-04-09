using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductVms;
public class ProductWithPurchaseRateVm
{
    public long? ProductId { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
    public string ProductVATorNOT { get; set; }
    public decimal? Stock_Quantity { get; set; }
    public long? UnitId { get; set; }
    public Unit? Unit { get; set; }
    public long? CategoryId { get; set; }
    public Category? Category { get; set; }
    public decimal? PurchaseRateAmt { get; set; } //for only displaying on product index
}
