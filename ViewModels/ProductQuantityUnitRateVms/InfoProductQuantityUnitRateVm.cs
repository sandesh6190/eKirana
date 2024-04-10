using eKirana.Models;
using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
//for listing productQuantityUnitRate
public class InfoProductQuantityUnitRateVm
{
    // public long? ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal? Quanity { get; set; }
    // public long? UnitId { get; set; }
    public Unit? Unit { get; set; }
    public decimal? PurchaseRate { get; set; }
    public decimal? SaleRate { get; set; }
}
