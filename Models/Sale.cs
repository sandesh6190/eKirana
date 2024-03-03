using eKirana.Models;

namespace eKirana;
public class Sale
{
    public long Id { get; set; }
    public string CustomerName { get; set; }
    public string CusromerAddress { get; set; }
    public DateTime SaleDate { get; set; }
    public long SaleById { get; set; }
    public virtual Admin Admins { get; set; }
}
