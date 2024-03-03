namespace eKirana.Models;
public class Purchase
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public virtual Supplier Suppliers { get; set; }
    public DateTime PurchaseDate { get; set; }
}
