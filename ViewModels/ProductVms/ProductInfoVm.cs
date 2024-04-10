using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductVms;
public class ProductInfoVm
{
    //In this Vm we can customize property to display on index page but right now here's no extra property to show thus, it has similar properties as product model 
    public long? ProductId { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
    public string ProductVATorNOT { get; set; }
    // public decimal? Stock_Quantity { get; set; }
    // public long? UnitId { get; set; }
    // public Unit? Unit { get; set; }
    // public long? CategoryId { get; set; }
    public Category? Category { get; set; }
    // public decimal? PurchaseRateAmt { get; set; } //for only displaying on product index
}
