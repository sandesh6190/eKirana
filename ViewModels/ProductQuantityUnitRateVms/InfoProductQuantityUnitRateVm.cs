using eKirana.Models;
using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
//for listing productQuantityUnitRate
public class InfoProductQuantityUnitRateVm
{
    public long? PrdQURId { get; set; }
    public Product? Product { get; set; } //string productName matra gardapani hunthyo tara entire product garda photo + name pani access garna milyo
    public long? Quantity { get; set; } //for showing on list

    public long? UnitId { get; set; }
    public string? UnitName { get; set; } //string unitName matra garda pani huncha
    public decimal? PurchaseRate { get; set; }
    public decimal? SaleRate { get; set; }
    public bool? IsBaseUnit { get; set; }
    public long Ratio { get; set; }
}
