using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms;
public class PurchaseFormAddVm //for formma dekhauna ko lagi matrei banayeko vm ho, arko PurchaseAddVmle actually kam garcha with the help of fetchapi
{
    public long SupplierId { get; set; }
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

    public DateTime PurchaseDate { get; set; } = DateTime.Now;//yo lakbak chaindeina

    public long? ProductId { get; set; }
    public List<Product>? Products;
    public SelectList ProductSelectList()
    {
        return new SelectList(
            Products,
            nameof(Product.Id),
            nameof(Product.Name),
            ProductId
        );
    }

    public long UnitId { get; set; }
    //unit is fetched through api
    // public List<Unit>? Units;
    // public SelectList UnitSelectList()
    // {
    //     return new SelectList(
    //     Units,
    //     nameof(Unit.Id),
    //     nameof(Unit.Name),
    //     UnitId
    //     );
    // }
}
