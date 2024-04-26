using eKirana.Constants;
using eKirana.Models;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp.MemberShipVms;
using Microsoft.AspNetCore.Mvc.Rendering;
//only for showing form for input
namespace eKirana.ViewModels.SaleVms;
public class SaleFormAddVm
{
    public string? CustomerType { get; set; }
    public SelectList CustomerTypeSelectList()
    {
        return new SelectList(
            CustomerTypeConstants.CustomerTypeList,
            CustomerType
        );
    }
    public DateTime SaleDate { get; set; } = DateTime.Now;

    public long? MemberShipId { get; set; }
    public List<MemberShipInfoVm>? MemberShipInfoVms;
    public SelectList MemberSelectList()
    {
        return new SelectList(
            MemberShipInfoVms,
            nameof(MemberShipInfoVm.MemberShipId),
            nameof(MemberShipInfoVm.Name),
            MemberShipId
        );
    }
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
    //unit is fetched through api
    // public List<Unit> Units;
    // public SelectList UnitSelectList()
    // {
    //     return new SelectList(
    //         Units,
    //         nameof(Unit.Id),
    //         nameof(Unit.Name),
    //         UnitId
    //     );
    // }
}
