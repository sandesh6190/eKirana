using eKirana.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SetUp.MemberShipVms;
public class MemberShipIndexVm
{
    public string SearchString { get; set; }
    public string SearchNumber { get; set; }
    public string SearchMemberShipStatus { get; set; }
    public SelectList MemberShipStatusSelectList()
    {
        return new SelectList(
            MemberShipStatusConstants.MemberShipStatusList,
            SearchMemberShipStatus
        );
    }
    public List<MemberShipInfoVm> MemberShipInfoVms { get; set; }
}
