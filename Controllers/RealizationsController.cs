using CRM_Duo_Creative.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Globalization;

namespace CRM_Duo_Creative.Controllers
{
    [Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager, Pracownik")]
    public class RealizationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RealizationsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string month = null, int? editId = null, int page = 1, int pageSize = 10, string? assignedTo = null)
        {
            ViewBag.SelectedAssignedTo = assignedTo;
            var user = await _userManager.GetUserAsync(User);
            var isPrivileged = User.IsInRole("Admin") || User.IsInRole("Dyrektor") || User.IsInRole("Kierownik") || User.IsInRole("Manager");

            DateTime targetMonth = string.IsNullOrEmpty(month)
                ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                : DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);

            var realizationsQuery = _context.Realizations
                .Include(r => r.Client)
                .Include(r => r.Statuses)
                .Where(r =>
                    !r.IsArchived &&
                    r.Client.ServiceStartDate.HasValue &&
                    r.Client.ServiceEndDate.HasValue &&
                    r.Client.ServiceStartDate.Value <= targetMonth.AddMonths(1).AddDays(-1) &&
                    r.Client.ServiceEndDate.Value >= new DateTime(targetMonth.Year, targetMonth.Month, 1)
                );
            if (!string.IsNullOrEmpty(assignedTo))
            {
                realizationsQuery = realizationsQuery.Where(r => r.AssignedTo == assignedTo);
            }
            if (!isPrivileged)
            {
                realizationsQuery = realizationsQuery.Where(r => r.AssignedTo == user.Id);
            }

            var totalItems = await realizationsQuery.CountAsync();
            var realizations = await realizationsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var r in realizations)
            {
                if (!string.IsNullOrEmpty(r.AssignedTo))
                {
                    var assignedUser = await _userManager.FindByIdAsync(r.AssignedTo);
                    if (assignedUser != null)
                    {
                        r.Client.AccessGrantedBy = $"{assignedUser.FirstName} {assignedUser.LastName}";
                    }
                }
            }
            var allUsers = await _userManager.Users.ToListAsync();

            ViewBag.Users = allUsers;
            ViewBag.SelectedAssignedTo = assignedTo;
            ViewBag.SelectedMonth = targetMonth.ToString("yyyy-MM");
            ViewBag.EditingId = editId;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(realizations);
        }


        [Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager")]
        public async Task<IActionResult> Archived(string search, int page = 1, int pageSize = 10)
        {
            var query = _context.Realizations
                .Include(r => r.Client)
                .Where(r => r.IsArchived);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    r.Client.ClientName.Contains(search) ||
                    r.Client.Services.Contains(search) ||
                    r.Client.Packages.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var realizations = await query
                .OrderByDescending(r => r.Month)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.Search = search;

            return View(realizations);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status, string month, int page, int pageSize, string assignedTo)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var realization = await _context.Realizations
                .Include(r => r.Statuses)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (realization == null)
                return NotFound();

            var selectedMonth = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);

            var statusEntry = realization.Statuses.FirstOrDefault(s => s.Month == selectedMonth);

            if (statusEntry == null)
            {
                statusEntry = new RealizationStatus
                {
                    RealizationId = realization.Id,
                    Month = selectedMonth,
                    Status = status
                };
                _context.RealizationStatuses.Add(statusEntry);
            }
            else
            {
                statusEntry.Status = status;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new {
                month = selectedMonth.ToString("yyyy-MM"),
                page = page,
                pageSize = pageSize,
                assignedTo = assignedTo,
                editId = (int?)null
            });
        }

        [Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager")]
        public async Task<IActionResult> Assign(int id)
        {
            var realization = await _context.Realizations
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (realization == null)
                return NotFound();

            // Pobierz WSZYSTKICH aktywnych użytkowników
            var activeUsers = await _userManager.Users.ToListAsync();

            ViewBag.Users = activeUsers;
            return View(realization);
        }


        [Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager")]
        [HttpPost]
        public async Task<IActionResult> Assign(int id, string userId)
        {
            var realization = await _context.Realizations.FindAsync(id);
            if (realization == null) return NotFound();

            realization.AssignedTo = userId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { month = realization.Month.ToString("yyyy-MM") });
        }

        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            var realization = await _context.Realizations.FindAsync(id);
            if (realization == null) return NotFound();

            var client = await _context.Clients.FindAsync(realization.ClientId);
            if (client != null) client.IsArchived = true;

            realization.IsArchived = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var realization = await _context.Realizations.FindAsync(id);
            if (realization == null) return NotFound();

            var client = await _context.Clients.FindAsync(realization.ClientId);
            if (client != null) client.IsArchived = false;

            realization.IsArchived = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { month = realization.Month.ToString("yyyy-MM") });
        }
       
    }
}
