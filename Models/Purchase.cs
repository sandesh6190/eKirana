using System.ComponentModel.DataAnnotations.Schema;
using eKirana.Models.SetUp;

namespace eKirana.Models;
public class Purchase
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
    public DateTime PurchaseDate { get; set; }
    public long PurchaseById { get; set; }
    [ForeignKey("PurchaseById")]
    public virtual Admin Admin { get; set; }
    public decimal TotalPaidAmount { get; set; }
}
