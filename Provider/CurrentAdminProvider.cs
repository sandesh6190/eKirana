using System.Security.Claims;
using eKirana.Data;
using eKirana.Models;
using eKirana.Provider.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Provider;
public class CurrentAdminProvider : ICurrentAdminProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ApplicationDbContext _context;

    public CurrentAdminProvider(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _contextAccessor = httpContextAccessor;
        _context = context;
    }
    public bool IsLoggedIn()
    => GetCurrentAdminId() != null;
    public async Task<Admin?> GetCurrentAdmin()
    {
        var currentAdminId = GetCurrentAdminId();
        if (!currentAdminId.HasValue) return null;

        return await _context.Admins.FindAsync(currentAdminId.Value);
    }

    public long? GetCurrentAdminId()
    {
        var adminId = _contextAccessor.HttpContext?.User.FindFirstValue("Id"); //HttpContext class ma chai User property nei raicha hai.
        if (string.IsNullOrWhiteSpace(adminId)) return null;

        return Convert.ToInt64(adminId);
    }
}
