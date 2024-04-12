using eKirana.Models;
using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
//for listing productQuantityUnitRate
public class InfoProductQuantityUnitRateVm
{
    public long? PrdQURId { get; set; }
    public Product? Product { get; set; } //string productName matra gardapani hunthyo tara entire product garda photo + name pani access garna milyo
    public decimal? Quantity { get; set; }
    public long? UnitId { get; set; }
    public Unit? Unit { get; set; } //string unitName matra garda pani hunthyo
    public decimal? PurchaseRate { get; set; }
    public decimal? SaleRate { get; set; }
    public bool? IsBaseUnit { get; set; }
    public decimal? Ratio { get; set; }
}
