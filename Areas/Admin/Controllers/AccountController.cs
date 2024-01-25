using Microsoft.AspNetCore.Mvc;

namespace Revas.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
