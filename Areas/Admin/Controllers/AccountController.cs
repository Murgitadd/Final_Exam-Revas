using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revas.Areas.Admin.ViewModels;
using Revas.Models;

namespace Revas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);

            }
            AppUser user = new AppUser
            {
                Name = vm.Name,
                Email = vm.Email,
                UserName = vm.Username,
                Surname = vm.Surname,

            };
            var res = await _userManager.CreateAsync(user, vm.Password);
            if (!res.Succeeded)
            {
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> Create()
        {
            IdentityRole role = new IdentityRole
            {
                Name = "Admin"
            };
            await _roleManager.CreateAsync(role);

            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Wrong credentials");
                    return View(vm);
                }

            }
            var res = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
            if (res.IsLockedOut)
            {
                ModelState.AddModelError("", "Locked out");
                return View(vm);
            }
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials");
                return View(vm);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
