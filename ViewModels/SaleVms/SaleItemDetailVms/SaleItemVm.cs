namespace eKirana.ViewModels.SaleVms.SaleItemDetailVms;
public class SaleItemVm
{
    public long ProductId { get; set; }
    public decimal Quantity { get; set; }
    public long UnitId { get; set; }
    public decimal Rate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal VATAmt { get; set; }
    public decimal DisAmt { get; set; }
    public decimal NetAmt { get; set; }
}
