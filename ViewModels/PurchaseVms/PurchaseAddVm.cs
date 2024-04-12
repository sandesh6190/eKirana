using eKirana.Models.SetUp;
using eKirana.ViewModels.PurchaseVms.PurchaseDetailVms;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.PurchaseVms;
public class PurchaseAddVm
{
    public DateTime PurchaseDate { get; set; }
    public long SupplierId { get; set; }
    public List<PurchaseItemVm> PurchaseItems { get; set; }
}
