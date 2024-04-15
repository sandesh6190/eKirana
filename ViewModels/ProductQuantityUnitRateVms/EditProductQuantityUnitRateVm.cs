using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
public class EditProductQuantityUnitRateVm
{
    public long ProductId; //only getting from code not from user input
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
