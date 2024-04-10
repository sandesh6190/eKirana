using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
public class IndexProductQuantityUnitRateVm
{
    //this vm is especially for searching only
    //for searching through unit
    public long? UnitId { get; set; }
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

    //for listing producQuantityUnitRate
    public List<InfoProductQuantityUnitRateVm> InfoProductQuantityUnitRateVms; //yaha list of model garda pan hunthyo but would be bad practise
}
