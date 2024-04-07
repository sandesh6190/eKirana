using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms;
public class PurchaseIndexVm
{

    public DateTime? FromPurchaseDate { get; set; }
    public DateTime? ToPurchaseDate { get; set; }
    public long? SupplierId { get; set; }
    public List<Supplier>? Suppliers;
    public SelectList SupplierSelectList()
    {
        return new SelectList(
            Suppliers,
            nameof(Supplier.Id),
            nameof(Supplier.Name),
            SupplierId
        );
    }
    public long? PurchaseById { get; set; }
    public List<Admin>? Admins;
    public SelectList AdminSelectList()
    {
        return new SelectList(
        Admins,
        nameof(Admin.Id),
        nameof(Admin.UserName),
        PurchaseById
        );
    }
    public decimal TotalPaidAmount { get; set; }
    public List<Purchase> Purchases;
}
