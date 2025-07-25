using CRM_Duo_Creative.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


[Authorize(Roles = "Admin, Dyrektor, Kierownik, Manager, Pracownik")]
public class LeadsController : Controller
{
    private readonly AppDbContext _context;

    public LeadsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
    {
        var query = _context.Leads.Where(l => !l.IsConverted);
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(l =>
                l.ClientName.Contains(search) ||
                l.Source.Contains(search) ||
                l.Services.Contains(search) ||
                l.Packages.Contains(search) ||
                l.ContractNumber.Contains(search) ||
                l.ContactPerson.Contains(search) ||
                l.ContactData.Contains(search));
        }

        int totalItems = await query.CountAsync();

        var leads = await query
            .OrderByDescending(l => l.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalItems = totalItems;
        ViewBag.Search = search;

        return View(leads);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Lead updatedLead)
    {
        var lead = await _context.Leads.FindAsync(updatedLead.Id);
        if (lead == null) return NotFound();

        lead.ClientName = updatedLead.ClientName;
        lead.Source = updatedLead.Source;
        lead.Services = updatedLead.Services;
        lead.Packages = updatedLead.Packages;
        lead.ContractNumber = updatedLead.ContractNumber;
        lead.ServiceStartDate = updatedLead.ServiceStartDate;
        lead.ServiceEndDate = updatedLead.ServiceEndDate;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ConvertToClient(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        if (string.IsNullOrWhiteSpace(lead.ClientName) || string.IsNullOrWhiteSpace(lead.Source) ||
            string.IsNullOrWhiteSpace(lead.Services) || string.IsNullOrWhiteSpace(lead.Packages) ||
            string.IsNullOrWhiteSpace(lead.ContractNumber) || !lead.ServiceStartDate.HasValue || !lead.ServiceEndDate.HasValue)
        {
            TempData["Error"] = "Uzupełnij wszystkie wymagane dane przed dodaniem do bazy klientów.";
            return RedirectToAction("Index");
        }

        var client = new Client
        {
            ClientName = lead.ClientName,
            Source = lead.Source,
            Services = lead.Services!,
            Packages = lead.Packages!,
            ContractNumber = string.IsNullOrWhiteSpace(lead.ContractNumber) ? "Brak" : lead.ContractNumber,
            ContactPerson = string.IsNullOrWhiteSpace(lead.ContactPerson) ? "Brak" : lead.ContactPerson,
            ContactData = string.IsNullOrWhiteSpace(lead.ContactData) ? "Brak" : lead.ContactData,
            AccessGrantedBy = string.IsNullOrWhiteSpace(lead.AccessGrantedBy) ? "Brak" : lead.AccessGrantedBy,
            MetaAccount = string.IsNullOrWhiteSpace(lead.MetaAccount) ? "Brak" : lead.MetaAccount,
            MetaAgreement = string.IsNullOrWhiteSpace(lead.MetaAgreement) ? "Brak" : lead.MetaAgreement,
            LocalData = string.IsNullOrWhiteSpace(lead.LocalData) ? "Brak" : lead.LocalData,
            SocialWebGoogleDrive = string.IsNullOrWhiteSpace(lead.SocialWebGoogleDrive) ? "Brak" : lead.SocialWebGoogleDrive,
            Notes = string.IsNullOrWhiteSpace(lead.Notes) ? "Brak" : lead.Notes,
            ServiceStartDate = lead.ServiceStartDate!.Value,
            ServiceEndDate = lead.ServiceEndDate!.Value,
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        var realization = new Realization
        {
            ClientId = client.Id,
            Month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
            Status = "Brak Danych"
        };
        _context.Realizations.Add(realization);
        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lead lead, List<string> SelectedServices, List<string> SelectedPackages)
    {
        if (string.IsNullOrWhiteSpace(lead.ClientName) || string.IsNullOrWhiteSpace(lead.Source))
        {
            ModelState.AddModelError(string.Empty, "Nazwa klienta i pochodzenie są wymagane.");
        }

        // Sklej wybrane usługi i pakiety w stringi (np. "Usługa1, Usługa2")
        lead.Services = SelectedServices != null ? string.Join(", ", SelectedServices) : string.Empty;
        lead.Packages = SelectedPackages != null ? string.Join(", ", SelectedPackages) : string.Empty;

        if (!ModelState.IsValid)
        {
            ViewBag.Services = await _context.Services.Select(s => s.Name).ToListAsync();
            ViewBag.Packages = await _context.Packages.Select(p => p.Name).ToListAsync();
            return View(lead);
        }

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Services = await _context.Services.Select(s => s.Name).ToListAsync();
        ViewBag.Packages = await _context.Packages.Select(p => p.Name).ToListAsync();
        return View(new Lead());
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null)
            return NotFound();

        ViewBag.Services = await _context.Services.Select(s => s.Name).ToListAsync();
        ViewBag.Packages = await _context.Packages.Select(p => p.Name).ToListAsync();

        return View(lead);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lead lead, List<string> SelectedServices, List<string> SelectedPackages)
    {
        if (id != lead.Id)
            return NotFound();

        if (string.IsNullOrWhiteSpace(lead.ClientName) || string.IsNullOrWhiteSpace(lead.Source))
        {
            ModelState.AddModelError(string.Empty, "Nazwa klienta i pochodzenie są wymagane.");
        }

        lead.Services = SelectedServices != null ? string.Join(", ", SelectedServices) : string.Empty;
        lead.Packages = SelectedPackages != null ? string.Join(", ", SelectedPackages) : string.Empty;

        if (!ModelState.IsValid)
        {
            ViewBag.Services = await _context.Services.Select(s => s.Name).ToListAsync();
            ViewBag.Packages = await _context.Packages.Select(p => p.Name).ToListAsync();
            return View(lead);
        }

        try
        {
            _context.Update(lead);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Leads.Any(e => e.Id == id))
                return NotFound();

            throw;
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private void PopulateServiceAndPackageSelectLists()
    {
        ViewBag.Services = _context.Services.Select(s => s.Name).ToList();
        ViewBag.Packages = _context.Packages.Select(p => p.Name).ToList();
    }
}
