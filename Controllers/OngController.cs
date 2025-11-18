using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    [Authorize(Roles = "ONG")]
    public class OngController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OngController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ONG/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var ong = await _context.ONGs.FirstOrDefaultAsync(o => o.Id == userId);

            if (ong == null)
                return NotFound();

            if (ong.Status == "Pendente")
            {
                TempData["InfoMessage"] = "Sua ONG está aguardando validação do administrador.";
            }

            var projetos = await _context.Projetos
                .Where(p => p.OngId == userId)
                .ToListAsync();

            ViewBag.Projetos = projetos;
            return View(ong);
        }

        // GET: ONG/CriarProjeto
        public IActionResult CriarProjeto()
        {
            return View();
        }

        // POST: ONG/CriarProjeto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarProjeto(Projeto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);
            var ong = await _context.ONGs.FirstOrDefaultAsync(o => o.Id == userId);

            if (ong == null || ong.Status == "Pendente")
            {
                TempData["ErrorMessage"] = "Apenas ONGs validadas podem criar projetos!";
                return RedirectToAction("Dashboard");
            }

            model.OngId = userId!;
            _context.Projetos.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Projeto criado com sucesso!";
            return RedirectToAction("Dashboard");
        }

        // GET: ONG/EditarProjeto/{id}
        public async Task<IActionResult> EditarProjeto(int id)
        {
            var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (projeto.OngId != userId)
                return Forbid();

            return View(projeto);
        }

        // POST: ONG/EditarProjeto/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProjeto(int id, Projeto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (projeto.OngId != userId)
                return Forbid();

            projeto.Titulo = model.Titulo;
            projeto.Descricao = model.Descricao;
            projeto.Categoria = model.Categoria;
            projeto.Endereco = model.Endereco;
            projeto.Data = model.Data;
            projeto.TotalVagas = model.TotalVagas;

            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Projeto atualizado com sucesso!";
            return RedirectToAction("Dashboard");
        }

        // POST: ONG/DeletarProjeto/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarProjeto(int id)
        {
            var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (projeto.OngId != userId)
                return Forbid();

            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Projeto deletado com sucesso!";
            return RedirectToAction("Dashboard");
        }

        // GET: ONG/ListaCandidatos/{id}
        public async Task<IActionResult> ListaCandidatos(int id)
        {
            var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (projeto.OngId != userId)
                return Forbid();

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Voluntario)
                .Where(c => c.ProjetoId == id)
                .ToListAsync();

            ViewBag.Projeto = projeto;
            return View(candidaturas);
        }

        // POST: ONG/AceitarCandidato/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AceitarCandidato(int id)
        {
            var candidatura = await _context.Candidaturas
                .Include(c => c.Projeto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidatura == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (candidatura.Projeto!.OngId != userId)
                return Forbid();

            candidatura.Status = "Aceita";
            _context.Candidaturas.Update(candidatura);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Candidato aceito com sucesso!";
            return RedirectToAction("ListaCandidatos", new { id = candidatura.ProjetoId });
        }

        // POST: ONG/RecusarCandidato/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecusarCandidato(int id)
        {
            var candidatura = await _context.Candidaturas
                .Include(c => c.Projeto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidatura == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (candidatura.Projeto!.OngId != userId)
                return Forbid();

            candidatura.Status = "Recusada";
            _context.Candidaturas.Update(candidatura);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Candidato recusado!";
            return RedirectToAction("ListaCandidatos", new { id = candidatura.ProjetoId });
        }
    }
}
