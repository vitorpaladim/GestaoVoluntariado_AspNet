using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    [Authorize(Roles = "Voluntario")]
    public class VoluntarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VoluntarioController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Voluntario/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.Id == userId);

            if (voluntario == null)
                return NotFound();

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Projeto)
                .Where(c => c.VoluntarioId == userId)
                .ToListAsync();

            ViewBag.Candidaturas = candidaturas;
            return View(voluntario);
        }

        // GET: Voluntario/EditPerfil
        public async Task<IActionResult> EditPerfil()
        {
            var userId = _userManager.GetUserId(User);
            var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.Id == userId);

            if (voluntario == null)
                return NotFound();

            return View(voluntario);
        }

        // POST: Voluntario/EditPerfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPerfil(Voluntario model, IFormFile? foto, IFormFile? curriculo)
        {
            var userId = _userManager.GetUserId(User);
            var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.Id == userId);

            if (voluntario == null)
                return NotFound();

            voluntario.Nome = model.Nome;
            voluntario.Telefone = model.Telefone;

            // Processar upload de foto
            if (foto != null && foto.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(fileStream);
                }

                voluntario.FotoPath = "/uploads/" + fileName;
            }

            // Processar upload de currículo
            if (curriculo != null && curriculo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(curriculo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await curriculo.CopyToAsync(fileStream);
                }

                voluntario.CurriculoPath = "/uploads/" + fileName;
            }

            _context.Voluntarios.Update(voluntario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Dashboard");
        }

        // GET: Voluntario/ListaProjetos
        public async Task<IActionResult> ListaProjetos(string? cidade = null, string? causa = null, string? palavra = null)
        {
            var projetos = _context.Projetos
                .Include(p => p.ONG)
                .Where(p => p.ONG!.Status == "Validada")
                .AsQueryable();

            if (!string.IsNullOrEmpty(cidade))
                projetos = projetos.Where(p => p.Endereco.Contains(cidade));

            if (!string.IsNullOrEmpty(causa))
                projetos = projetos.Where(p => p.Categoria.Contains(causa));

            if (!string.IsNullOrEmpty(palavra))
                projetos = projetos.Where(p => p.Titulo.Contains(palavra) || p.Descricao.Contains(palavra));

            var resultado = await projetos.ToListAsync();

            // Incrementar views
            foreach (var projeto in resultado)
            {
                projeto.Views++;
            }
            await _context.SaveChangesAsync();

            ViewBag.Cidade = cidade;
            ViewBag.Causa = causa;
            ViewBag.Palavra = palavra;

            return View(resultado);
        }

        // GET: Voluntario/DetalhesProjeto/{id}
        public async Task<IActionResult> DetalhesProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.ONG)
                .Include(p => p.Candidaturas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var jaCandidatou = await _context.Candidaturas
                .AnyAsync(c => c.ProjetoId == id && c.VoluntarioId == userId);

            ViewBag.JaCandidatou = jaCandidatou;
            return View(projeto);
        }

        // POST: Voluntario/Candidatar/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Candidatar(int id)
        {
            var userId = _userManager.GetUserId(User);
            var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var jaCandidatou = await _context.Candidaturas
                .AnyAsync(c => c.ProjetoId == id && c.VoluntarioId == userId);

            if (jaCandidatou)
            {
                TempData["ErrorMessage"] = "Você já se candidatou para este projeto!";
                return RedirectToAction("DetalhesProjeto", new { id });
            }

            var candidatura = new Candidatura
            {
                ProjetoId = id,
                VoluntarioId = userId,
                Status = "Pendente"
            };

            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Candidatura enviada com sucesso!";
            return RedirectToAction("DetalhesProjeto", new { id });
        }
    }
}
