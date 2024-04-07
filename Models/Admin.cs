using eKirana.Constants;

namespace eKirana.Models;
public class Admin
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }
    public string AdminStatus { get; set; } = AdminStatusConstants.Active;
    public string AdminType { get; set; } = AdminTypeConstants.NormalAdmin;
    public DateTime Registered_On { get; set; }
}
