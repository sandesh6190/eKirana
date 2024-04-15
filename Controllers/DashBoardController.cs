using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eKirana.Controllers;
[Authorize(Roles = "NormalAdmin, SuperAdmin")]
public class DashBoardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
