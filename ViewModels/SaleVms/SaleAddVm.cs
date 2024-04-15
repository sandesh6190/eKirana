using eKirana.ViewModels.SaleVms.SaleItemDetailVms;

namespace eKirana.ViewModels.SaleVms;
public class SaleAddVm
{
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public DateTime SaleDate { get; set; }
    public List<SaleItemVm> SaleItemVms { get; set; }
}
