using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class InvoicesController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Strona w budowie";
        return View("UnderConstruction");
    }
}