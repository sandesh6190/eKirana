using Microsoft.AspNetCore.Mvc;

namespace eKirana.Controllers;
public class AuthenticationController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}
