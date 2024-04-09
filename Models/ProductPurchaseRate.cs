using eKirana.Models.SetUp;

namespace eKirana.Models;
public class ProductPurchaseRate
{
    public long? Id { get; set; }
    public long? ProductId { get; set; }
    public virtual Product Product { get; set; }
    public decimal? Amount { get; set; }
    public long? UnitId { get; set; }
    public virtual Unit? Unit { get; set; }
    public DateTime DateModified { get; set; }
}
