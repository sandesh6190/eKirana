using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;
//only for showing form for input
namespace eKirana.ViewModels.SaleVms;
public class SaleFormAddVm
{
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public long ProductId { get; set; }
    public List<Product> Products;
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
    public List<Unit> Units;
    public SelectList UnitSelectList()
    {
        return new SelectList(
            Units,
            nameof(Unit.Id),
            nameof(Unit.Name),
            UnitId
        );
    }
}
