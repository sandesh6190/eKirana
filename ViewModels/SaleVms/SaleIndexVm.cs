using eKirana.Constants;
using eKirana.Models;
using eKirana.ViewModels.SetUp.MemberShipVms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SaleVms;
public class SaleIndexVm
{
    public string? SearchCustomer { get; set; }
    public string? CustomerType { get; set; }
    public SelectList CustomerTypeSelectList()
    {
        return new SelectList(
            CustomerTypeConstants.CustomerTypeList,
            CustomerType
        );
    }
    public DateTime? FromSaleDate { get; set; }
    public DateTime? ToSaleDate { get; set; }
    // public long? MemberShipId { get; set; }
    // public List<MemberShipInfoVm> MemberShipInfoVms;
    // public SelectList MemberShipSelectList()
    // {
    //     return new SelectList(
    //         MemberShipInfoVms,
    //         nameof(MemberShipInfoVm.MemberShipId),
    //         nameof(MemberShipInfoVm.Name),
    //         MemberShipId
    //     );
    // }
    public long? SaleById { get; set; }
    public List<Admin>? Admins;
    public SelectList AdminSelectList()
    {
        return new SelectList(
        Admins,
        nameof(Admin.Id),
        nameof(Admin.UserName),
        SaleById
        );
    }

    //public List<Sale> Sales; //yesari model nei rakheko not better

    public List<SaleInfoVm> SaleInfoVms { get; set; } //duplicate of sale model

}
