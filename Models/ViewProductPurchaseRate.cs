namespace eKirana.Models;
public class ViewProductPurchaseRate
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
    public long ProductPurchaseRateId { get; set; }
    public virtual ProductPurchaseRate ProductPurchaseRate { get; set; }
}
