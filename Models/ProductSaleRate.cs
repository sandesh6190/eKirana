using eKirana.Models.SetUp;

namespace eKirana.Models;
public class ProductSaleRate
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public long UnitId { get; set; }
    public virtual Unit Units { get; set; }
    public DateTime DateModified { get; set; }
}
