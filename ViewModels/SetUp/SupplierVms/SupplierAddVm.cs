using eKirana.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SetUp.SupplierVms;
public class SupplierAddVm
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    // public string SupplierStatus { get; set; } // status: active for first time
    // public SelectList SupplierStatusSelectList()
    // {
    //     return new SelectList(
    //         SupplierStatusConstants.SupplierStatusList,
    //         SupplierStatus
    //     );
    // }

}
