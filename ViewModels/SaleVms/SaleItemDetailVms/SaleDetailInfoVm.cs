namespace eKirana.ViewModels.SaleVms.SaleItemDetailVms;
public class SaleDetailInfoVm
{
    public long SaleId { get; set; }
    public string ProductName { get; set; }
    public long Quantity { get; set; }
    public string UnitName { get; set; }
    public decimal Rate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal VATAmt { get; set; }
    public decimal DisAmt { get; set; }
    public decimal NetAmt { get; set; }
}
