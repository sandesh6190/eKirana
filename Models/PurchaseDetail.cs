using System.ComponentModel.DataAnnotations.Schema;
using eKirana.Models.SetUp;

namespace eKirana.Models;
public class PurchaseDetail
{
    public long? Id { get; set; }
    public long? PurchaseId { get; set; }
    public virtual Purchase Purchase { get; set; }
    public long? ProductId { get; set; }
    public virtual Product Product { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Rate { get; set; }
    // public long ProductPurchaseRateId { get; set; }
    // public virtual ProductPurchaseRate ProductPurchaseRate { get; set; }
    public long? UnitId { get; set; }
    //[ForeignKey("UnitId")]
    public virtual Unit Unit { get; set; }
    public decimal? SubTotal { get; set; }
    public decimal? NetAmount { get; set; }
    public decimal? VATAmount { get; set; }
    public decimal? Discount { get; set; }

}
