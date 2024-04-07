using eKirana.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SetUp.SupplierVms;
public class SupplierEditVm
{
    public string Name { get; set; }
    public string Address { get; set; }
    public long PhoneNumber { get; set; }
    public string SupplierStatus { get; set; }
    public SelectList SupplierStatusSelectList()
    {
        return new SelectList(
            SupplierStatusConstants.SupplierStatusList,
            SupplierStatus
        );
    }
}
