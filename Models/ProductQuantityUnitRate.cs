using eKirana.Models.SetUp;

namespace eKirana.Models;
public class ProductQuantityUnitRate
{
    public long Id { get; set; }
    public long Stock_Quantity { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
    public long UnitId { get; set; }
    public virtual Unit Unit { get; set; }
    public bool IsBaseUnit { get; set; }
    public long Ratio { get; set; }

}
