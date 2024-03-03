using eKirana.Constants;

namespace eKirana.Models;
public class Supplier
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public long PhoneNumber { get; set; }
    public string SupplierStatus { get; set; } = SupplierStatusConstants.Active;
}
