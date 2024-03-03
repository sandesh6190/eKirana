using Microsoft.AspNetCore.Mvc;

namespace eKirana.Controllers;
public class DashBoardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
