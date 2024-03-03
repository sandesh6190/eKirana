using eKirana.Models.SetUp;

namespace eKirana.Models;
public class SaleDetail
{
    public long Id { get; set; }
    public long SaleId { get; set; }
    public virtual Sale Sales { get; set; }
    public long ProductId { get; set; }
    public virtual Product Products { get; set; }
    public long ProductSaleRateId { get; set; }
    public virtual ProductSaleRate ProductSaleRates { get; set; }
    public long UnitId { get; set; }
    public virtual Unit Units { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidAmount { get; set; }
}
