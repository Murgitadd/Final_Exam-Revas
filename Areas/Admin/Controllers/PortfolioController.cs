using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Revas.Areas.Admin.ViewModels;
using Revas.DAL;
using Revas.Models;
using Revas.Utility.Extensions;
using System.Drawing;

namespace Revas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int total = await _db.Portfolios.CountAsync();
            int limit = 3;
            int tp = total / limit;
            if (total % limit > 0)
            {
                tp++;
            }
            if (tp != 0)
            {
                if (page > tp || page <= 0)
                {
                    return await Index(1);
                }

            }


            PaginationVM<Portfolio> vm = new PaginationVM<Portfolio>
            {
                Items = await _db.Portfolios.Skip((page - 1) * limit).Take(limit).ToListAsync(),
                TotalPage = tp,
                CurrentPage = page,
                Limit = limit
            };

            return View(vm);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePortfolioVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);

            }
            if (vm.Photo.CheckSize(2))
            {
                ModelState.AddModelError("Photo", "Cannot exceed 2 Mb");
                return View(vm);
            }
            if (vm.Photo.CheckType("image"))
            {
                ModelState.AddModelError("Photo", "Only images allowed");
                return View(vm);
            }
            Portfolio portfolio = new Portfolio()
            {
                Image = await vm.Photo.CreateFile(_env.WebRootPath, "assets", "img")
            };
            await _db.Portfolios.AddAsync(portfolio);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Portfolio portfolio = await _db.Portfolios.FirstOrDefaultAsync(x => x.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            UpdatePortfolioVM vm = new UpdatePortfolioVM
            {
               
                Image = portfolio.Image,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePortfolioVM vm)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Portfolio portfolio = await _db.Portfolios.FirstOrDefaultAsync(x => x.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Photo is not null)
            {
                if (vm.Photo.CheckSize(2))
                {
                    ModelState.AddModelError("Photo", "Cannot exceed 2 Mb");
                    return View(vm);
                }
                if (vm.Photo.CheckType("image"))
                {
                    ModelState.AddModelError("Photo", "Only images allowed");
                    return View(vm);
                }
                portfolio.Image.Delete(_env.WebRootPath, "assets", "img");
                portfolio.Image = await vm.Photo.CreateFile(_env.WebRootPath, "assets", "img");
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Portfolio portfolio = await _db.Portfolios.FirstOrDefaultAsync(x => x.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            portfolio.Image.Delete(_env.WebRootPath, "assets", "img");
            _db.Portfolios.Remove(portfolio);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
