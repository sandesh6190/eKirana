using System.ComponentModel.DataAnnotations.Schema;
using eKirana.Models.SetUp;

namespace eKirana.Models;
public class PurchaseDetail
{
    public long Id { get; set; }
    public long PurhaseId { get; set; }
    public virtual Purchase Purchases { get; set; }
    public long ProductId { get; set; }
    public virtual Product Products { get; set; }
    public long ProductPurchaseRateId { get; set; }
    public virtual ProductPurchaseRate ProductPurchaseRates { get; set; }
    public long UnitId { get; set; }
    //[ForeignKey("UnitId")]
    public virtual Unit Units { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal PaidAmount { get; set; }
}
