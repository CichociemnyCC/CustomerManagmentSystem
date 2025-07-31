using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CRM_Duo_Creative.Models;

namespace CRM_Duo_Creative.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index() => View();
        public IActionResult Users() => View();
        public IActionResult License() => View();

        public IActionResult Logs()
        {
            var logs = _context.LoginLogs
                        .OrderByDescending(l => l.LoginTime)
                        .ToList();
            return View(logs);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsBlocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now,
                CurrentRoles = roles.ToList(),
                AllRoles = allRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (model.IsBlocked)
                await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
            else
                await _userManager.SetLockoutEndDateAsync(user, null);

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("Users");

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(model);
        }

        public IActionResult CreateUser()
        {
            var model = new CreateUserViewModel
            {
                AllRoles = _roleManager.Roles.Select(r => r.Name).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.SelectedRoles?.Any() == true)
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);

                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Integrations()
        {
            var model = new IntegrationsViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveIntegrations(IntegrationsViewModel model)
        {
            TempData["Success"] = "Ustawienia zostały zapisane.";
            return RedirectToAction("Integrations");
        }

        // ------------------------------
        // PANEL USŁUGI + PAKIETY W JEDNYM
        // ------------------------------
        [HttpGet]
        public IActionResult ServicesPackages()
        {
            ViewBag.Services = _context.Services.OrderBy(s => s.Name).ToList();
            ViewBag.Packages = _context.Packages.OrderBy(p => p.Name).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddService(string serviceName)
        {
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                _context.Services.Add(new Service { Name = serviceName });
                _context.SaveChanges();
            }
            return RedirectToAction("ServicesPackages");
        }

        [HttpPost]
        public IActionResult AddPackage(string packageName)
        {
            if (!string.IsNullOrWhiteSpace(packageName))
            {
                _context.Packages.Add(new Package { Name = packageName });
                _context.SaveChanges();
            }
            return RedirectToAction("ServicesPackages");
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("ServicesPackages");
        }

        [HttpPost]
        public IActionResult DeletePackage(int id)
        {
            var package = _context.Packages.Find(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                _context.SaveChanges();
            }
            return RedirectToAction("ServicesPackages");
        }
        [HttpGet]
        public async Task<IActionResult> ChangeEmail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new ChangeEmailViewModel
            {
                UserId = user.Id,
                CurrentEmail = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            user.Email = model.NewEmail;
            user.UserName = model.NewEmail; // Jeśli login to e-mail

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Adres e-mail został zaktualizowany.";
                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

    }
}
