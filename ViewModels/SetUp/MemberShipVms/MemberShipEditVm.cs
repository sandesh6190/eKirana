using eKirana.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SetUp.MemberShipVms;
public class MemberShipEditVm
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public string MemberShipStatus { get; set; }
    public SelectList MemberShipStatusSelectList()
    {
        return new SelectList(
            MemberShipStatusConstants.MemberShipStatusList,
            MemberShipStatus
        );
    }
}
