using Microsoft.AspNetCore.Mvc;
using Revas.DAL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Revas.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Model> model = new List<Model>();
            return View(model);
        }
    }
}