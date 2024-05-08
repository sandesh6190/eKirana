namespace eKirana.Models;
public class StockQuantityHistory
{
    public long Id { get; set; }
    //public long ProductId { get; set; } product would be accessed through ProductQuantityUnitRate
    //public virtual Product Product { get; set; }
    public long ProductQuantityUnitRateId { get; set; }
    public virtual ProductQuantityUnitRate ProductQuantityUnitRate { get; set; }
    public long QuantityMovement { get; set; }
    public string Remarks { get; set; }
    public DateTime On_Date { get; set; }
    public long AdminId{get; set;}
    public virtual Admin Admin {get; set;}
}
