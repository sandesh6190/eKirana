using eKirana.Constants;

namespace eKirana.Models.SetUp;
public class Supplier
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string SupplierStatus { get; set; } = SupplierStatusConstants.Active;
    public DateTime? LastTransaction { get; set; }
}
