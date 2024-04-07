using eKirana.Constants;
using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductVms;
public class ProductIndexVm
{
    public string? Search { get; set; }
    public long? Min_Stock_Quantity { get; set; }
    public long? Max_Stock_Quantity { get; set; }
    //field to accept input
    public long? UnitId { get; set; }
    //displaying list of unit to proceed searching
    public List<Unit> Units;
    //select list of unit
    public SelectList UnitSelectList()
    {
        return new SelectList(
            Units,
            nameof(Unit.Id),
            nameof(Unit.Name)
        );
    }

    public string? ProductVATorNOT { get; set; }
    public SelectList ProductVATorNOTSelectList()
    {
        return new SelectList(
            ProductVATorNOTConstants.ProductTypeList,
            ProductVATorNOT
        );
    }

    //for product list to display
    public List<Product> Products;
    //public List<ProductPurchaseRate> ProductPurchaseRate;
}
