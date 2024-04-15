using System.Security.Claims;
using System.Transactions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Manager.Interfaces;
using eKirana.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Manager;
public class AuthManager : IAuthManager
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthManager(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogIn(string Email, string PassWord)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email.ToLower() == Email.ToLower().Trim());

        if (admin == null)
        {
            throw new Exception("Invalid Email.");
        }

        if (!BCrypt.Net.BCrypt.Verify(PassWord, admin.HashPassword))
        {
            throw new Exception("Invalid Password.");
        }

        var httpContext = _httpContextAccessor.HttpContext;
        var claims = new List<Claim>
        {
            new("Id", admin.Id.ToString()), //Id is used to represent the type of claim, claim stores the user's permission,roles,attributes and so on.
            //new Claim(CustomClaimTypes.AdminStatus, "Active")//customizing claim types
        };

        if (admin.AdminType == AdminTypeConstants.SuperAdmin)
        {
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "SuperAdmin"));
        }
        else if (admin.AdminType == AdminTypeConstants.NormalAdmin)
        {
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "NormalAdmin"));
        }
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

    }

    public async Task LogOut()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync();
    }

    public async Task Register(string UserName, string Email, string PassWord)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email.ToLower() == Email.ToLower().Trim());
        if (admin != null)
        {
            throw new Exception("Email Already Existed.");
        }

        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var newAdmin = new Admin();
        newAdmin.UserName = UserName;
        newAdmin.Email = Email;
        newAdmin.HashPassword = BCrypt.Net.BCrypt.HashPassword(PassWord);
        newAdmin.AdminStatus = AdminStatusConstants.Active;
        newAdmin.AdminType = AdminTypeConstants.NormalAdmin;
        newAdmin.Registered_On = DateTime.Now;

        _context.Admins.Add(newAdmin);
        await _context.SaveChangesAsync();

        tx.Complete();

    }



}

// customizing claim types
// public static class CustomClaimTypes
// {
//     public const string AdminStatus = "http://schemas.example.com/identity/claims/userstatus";
// }

