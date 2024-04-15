using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
public class AddProductQuantityUnitRateVm
{
    public long ProductId;
    public long Ratio { get; set; }
    public long UnitId { get; set; }
    public bool IsBaseUnit { get; set; }
    public List<Unit>? Units; //for selecting unit list
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
