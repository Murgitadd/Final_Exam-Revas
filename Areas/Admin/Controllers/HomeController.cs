using Microsoft.AspNetCore.Mvc;

namespace Revas.Areas.Admin.Controllers
{
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
