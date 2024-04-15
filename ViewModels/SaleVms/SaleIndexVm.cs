using eKirana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SaleVms;
public class SaleIndexVm
{
    public DateTime? FromSaleDate { get; set; }
    public DateTime? ToSaleDate { get; set; }
    public string? SearchCustomer { get; set; }
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
