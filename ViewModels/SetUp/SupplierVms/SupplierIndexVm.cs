using eKirana.Constants;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SetUp.SupplierVms;
public class SupplierIndexVm
{
    public string? SearchString { get; set; }
    public string? SupplierStatus { get; set; }
    public SelectList SupplierStatusSelectList()
    {
        return new SelectList(
            SupplierStatusConstants.SupplierStatusList,
            SupplierStatus
        );
    }

    public List<Supplier> Suppliers;

}
