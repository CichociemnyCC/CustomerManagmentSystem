using CRM_Duo_Creative.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CRM_Duo_Creative.Controllers
{
    [Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager, Pracownik")]
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search, string? status, DateTime? start, DateTime? end, int page = 1, int pageSize = 10)
        {
            var today = DateTime.Today;

            // Startowe zapytanie tylko na niearchiwizowanych klientach
            var query = _context.Clients.Where(c => !c.IsArchived).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.ClientName.Contains(search) ||
                    c.ContactPerson.Contains(search) ||
                    c.ContactData.Contains(search));
            }

            if (start.HasValue)
                query = query.Where(c => c.ServiceStartDate >= start);

            if (end.HasValue)
                query = query.Where(c => c.ServiceStartDate <= end);

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Aktywne")
                {
                    query = query.Where(c =>
                        c.ServiceStartDate.HasValue && c.ServiceEndDate.HasValue &&
                        today >= c.ServiceStartDate && today <= c.ServiceEndDate);
                }
                else if (status == "Oczekuje")
                {
                    query = query.Where(c =>
                        c.ServiceStartDate.HasValue && today < c.ServiceStartDate);
                }
                else if (status == "Zakończone")
                {
                    query = query.Where(c =>
                        c.ServiceEndDate.HasValue && today > c.ServiceEndDate);
                }
            }

            int totalItems = query.Count();

            var pagedClients = query
                .OrderBy(c => c.ClientName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(pagedClients);
        }

        public IActionResult Details(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();
            return View(client);
        }

        public IActionResult Create()
        {
            ViewBag.Services = _context.Services.Select(s => s.Name).ToList();
            ViewBag.Packages = _context.Packages.Select(p => p.Name).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            client.Services = string.Join(", ", Request.Form["SelectedServices"]);
            client.Packages = string.Join(", ", Request.Form["SelectedPackages"]);
            ModelState.Remove("Services");
            ModelState.Remove("Packages");
            FillEmptyFieldsWithBrak(client);

            if (client.ServiceStartDate.HasValue && client.ServiceEndDate.HasValue &&
                client.ServiceEndDate < client.ServiceStartDate)
            {
                ModelState.AddModelError("ServiceEndDate", "End date cannot be earlier than start date.");
            }

            if (ModelState.IsValid)
            {
                // Zapisz klienta
                _context.Clients.Add(client);
                _context.SaveChanges(); // klient.Id dostępne

                // Utwórz realizację
                var realization = new Realization
                {
                    ClientId = client.Id,
                    Month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                };
                _context.Realizations.Add(realization);
                _context.SaveChanges(); // realization.Id dostępne

                // Dodaj status powiązany z realizacją
                var status = new RealizationStatus
                {
                    RealizationId = realization.Id,
                    Month = realization.Month,
                    Status = "Brak Danych"
                };
                _context.RealizationStatuses.Add(status);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Services = _context.Services.Select(s => s.Name).ToList();
            ViewBag.Packages = _context.Packages.Select(p => p.Name).ToList();
            return View(client);
        }

        public IActionResult Edit(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            ViewBag.Services = _context.Services.Select(s => s.Name).ToList();
            ViewBag.Packages = _context.Packages.Select(p => p.Name).ToList();

            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            client.Services = string.Join(", ", Request.Form["SelectedServices"]);
            client.Packages = string.Join(", ", Request.Form["SelectedPackages"]);
            ModelState.Remove("Services");
            ModelState.Remove("Packages");
            FillEmptyFieldsWithBrak(client);

            if (client.ServiceStartDate.HasValue && client.ServiceEndDate.HasValue &&
                client.ServiceEndDate < client.ServiceStartDate)
            {
                ModelState.AddModelError("ServiceEndDate", "End date cannot be earlier than start date.");
            }

            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Services = _context.Services.Select(s => s.Name).ToList();
            ViewBag.Packages = _context.Packages.Select(p => p.Name).ToList();
            return View(client);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager")]    //autoryzacja archiwizacji
        public async Task<IActionResult> Archive(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            client.IsArchived = true;

            // Archiwizuj powiązane realizacje
            var realizations = _context.Realizations.Where(r => r.ClientId == id);
            foreach (var realization in realizations)
            {
                realization.IsArchived = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult ArchiveList(string search, int page = 1, int pageSize = 10)
        {
            var clients = _context.Clients.Where(c => c.IsArchived).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                clients = clients.Where(c =>
                    c.ClientName.Contains(search) ||
                    c.ContactPerson.Contains(search) ||
                    c.ContactData.Contains(search)).ToList();
            }

            int totalItems = clients.Count;
            var pagedClients = clients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(pagedClients);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client != null)
            {
                var realizations = _context.Realizations
                    .Where(r => r.ClientId == client.Id)
                    .ToList();
                _context.Realizations.RemoveRange(realizations);
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }

            return RedirectToAction("ArchiveList");
        }

        [HttpPost]
        public IActionResult Reactivate(int id)
        {
            var client = _context.Clients.Find(id);
            if (client != null)
            {
                client.IsArchived = false;
                var realizations = _context.Realizations.Where(r => r.ClientId == client.Id).ToList();
                foreach (var r in realizations)
                {
                    r.IsArchived = false;
                }

                _context.SaveChanges();
            }
            return RedirectToAction("ArchiveList");
        }

        private void FillEmptyFieldsWithBrak(Client client)
        {
            client.ContractNumber = string.IsNullOrWhiteSpace(client.ContractNumber) ? "Brak" : client.ContractNumber;
            client.ContactPerson = string.IsNullOrWhiteSpace(client.ContactPerson) ? "Brak" : client.ContactPerson;
            client.ContactData = string.IsNullOrWhiteSpace(client.ContactData) ? "Brak" : client.ContactData;
            client.AccessGrantedBy = string.IsNullOrWhiteSpace(client.AccessGrantedBy) ? "Brak" : client.AccessGrantedBy;
            client.MetaAccount = string.IsNullOrWhiteSpace(client.MetaAccount) ? "Brak" : client.MetaAccount;
            client.MetaAgreement = string.IsNullOrWhiteSpace(client.MetaAgreement) ? "Brak" : client.MetaAgreement;
            client.LocalData = string.IsNullOrWhiteSpace(client.LocalData) ? "Brak" : client.LocalData;
            client.SocialWebGoogleDrive = string.IsNullOrWhiteSpace(client.SocialWebGoogleDrive) ? "Brak" : client.SocialWebGoogleDrive;
            client.Notes = string.IsNullOrWhiteSpace(client.Notes) ? "Brak" : client.Notes;
        }
    }
}
