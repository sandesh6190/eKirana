using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.StockQuantityVms;
public class StockQuantityResetVm
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductBrand { get; set; }
    public long? StockQuantity { get; set; }
    public long UnitId { get; set; }
    public List<Unit>? Units { get; set; }
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

