namespace eKirana.Models;
public class SuperAdmin
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string HashPassword { get; set; }
    public DateTime Created_on { get; set; }
}
