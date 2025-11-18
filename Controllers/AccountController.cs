using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GestaoVoluntariado.Models;
using GestaoVoluntariado.ViewModels;

namespace GestaoVoluntariado.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Senha, model.LembrarMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                    return RedirectToAction("Index", "Admin");
                else if (roles.Contains("ONG"))
                    return RedirectToAction("Dashboard", "ONG");
                else if (roles.Contains("Voluntario"))
                    return RedirectToAction("Dashboard", "Voluntario");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            return View(model);
        }

        // GET: Account/RegisterVoluntario
        public IActionResult RegisterVoluntario()
        {
            return View(new RegisterVoluntarioViewModel());
        }

        // POST: Account/RegisterVoluntario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterVoluntario(RegisterVoluntarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var voluntario = new Voluntario
            {
                UserName = model.Email,
                Email = model.Email,
                Nome = model.Nome,
                Telefone = model.Telefone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(voluntario, model.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(voluntario, "Voluntario");
                await _signInManager.SignInAsync(voluntario, isPersistent: false);
                return RedirectToAction("Dashboard", "Voluntario");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Account/RegisterONG
        public IActionResult RegisterONG()
        {
            return View(new RegisterONGViewModel());
        }

        // POST: Account/RegisterONG
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterONG(RegisterONGViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Verificar adminCode
            string status = model.AdminCode == "gestaomaster" ? "Validada" : "Pendente";

            // Processar upload de documento
            string? documentoPath = null;
            if (model.Documento != null && model.Documento.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Documento.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Documento.CopyToAsync(fileStream);
                }

                documentoPath = "/uploads/" + fileName;
            }

            var ong = new ONG
            {
                UserName = model.Email,
                Email = model.Email,
                NomeONG = model.NomeONG,
                Telefone = model.Telefone,
                Endereco = model.Endereco,
                Descricao = model.Descricao,
                DocumentoPath = documentoPath,
                Status = status,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(ong, model.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(ong, "ONG");

                if (status == "Validada")
                {
                    TempData["SuccessMessage"] = "ONG registrada e validada com sucesso!";
                    await _signInManager.SignInAsync(ong, isPersistent: false);
                    return RedirectToAction("Dashboard", "ONG");
                }
                else
                {
                    TempData["InfoMessage"] = "ONG registrada! Aguardando validação do administrador.";
                    return RedirectToAction("Index", "Home");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
