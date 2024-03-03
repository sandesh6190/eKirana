using eKirana.Models.SetUp;

namespace eKirana.Models;
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long Stock_Quantity { get; set; }
    public long UnitId { get; set; }
    //public virtual Unit Units { get; set; }
    public long CategoryId { get; set; }
    public virtual Category Categories { get; set; }
    public long ProductPurchaseRateId { get; set; }
    public virtual ProductPurchaseRate ProductPurchaseRates { get; set; }
    public long ProductSaleRateId { get; set; }
    public virtual ProductSaleRate ProductSalesRateId { get; set; }
}
