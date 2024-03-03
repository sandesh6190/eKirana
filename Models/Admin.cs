namespace eKirana.Models;
public class Admin
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }
    public string AdminStatus { get; set; } = AdminStatusConstants.Active;
    public DateTime Created_on { get; set; }
}
