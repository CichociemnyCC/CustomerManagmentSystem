using CRM_Duo_Creative.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Manage()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(new ManageUserViewModel
        {
            Email = user.Email
        });
    }

    [HttpPost]
    public async Task<IActionResult> Manage(ManageUserViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (!ModelState.IsValid) return View(model);

        // Zmiana e-maila
        if (user.Email != model.Email)
        {
            user.Email = model.Email;
            user.UserName = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Błąd podczas aktualizacji adresu e-mail.");
                return View(model);
            }
        }

        // Zmiana hasła
        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Aby zmienić hasło, wpisz obecne hasło.");
                return View(model);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError("CurrentPassword", "Podane obecne hasło jest nieprawidłowe.");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Błąd podczas zmiany hasła.");
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user); // Odśwież login
        }

        TempData["Success"] = "Dane zostały zaktualizowane.";
        return RedirectToAction("Manage");
    }
}
