using eKirana.Models;

namespace eKirana.Provider.Interfaces;
public interface ICurrentAdminProvider
{
    bool IsLoggedIn();
    Task<Admin> GetCurrentAdmin();
    long? GetCurrentAdminId();
}
