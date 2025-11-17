using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GestaoVoluntariado.ViewModels; // <-- ADICIONADO

namespace GestaoVoluntariado.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public IActionResult Login()
        {
            return View(new LoginViewModel()); // <-- MODIFICADO
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) // <-- MODIFICADO
        {
            // MODIFICADO: Trocamos o 'if' antigo por este
            if (!ModelState.IsValid)
            {
                return View(model); // Retorna o modelo para mostrar os erros de validação
            }

            // Simple authentication: create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email), // <-- MODIFICADO (usando model.Email)
                new Claim(ClaimTypes.Name, model.Email)  // <-- MODIFICADO (usando model.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}