using eKirana.Models;
using eKirana.Models.SetUp;

namespace eKirana.ViewModels.SaleVms;
//duplicate of sale model
public class SaleInfoVm
{
    public long SaleId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public MemberShipHolder MemberShipHolder { get; set; }
    public DateTime SaleDate { get; set; }
    public string SaleBy { get; set; }
    //public Admin Admin { get; set; } admin model bata multiple data nikalda chai gareko ramro
    public decimal TotalAmount { get; set; }
}
