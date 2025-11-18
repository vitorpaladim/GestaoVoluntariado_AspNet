using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            var ongsNaoValidadas = await _context.ONGs
                .Where(o => o.Status == "Pendente")
                .ToListAsync();

            var ongsValidadas = await _context.ONGs
                .Where(o => o.Status == "Validada")
                .ToListAsync();

            ViewBag.ONGsNaoValidadas = ongsNaoValidadas;
            ViewBag.ONGsValidadas = ongsValidadas;

            return View();
        }

        // POST: Admin/ValidarONG/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarONG(string id)
        {
            var ong = await _context.ONGs.FirstOrDefaultAsync(o => o.Id == id);

            if (ong == null)
                return NotFound();

            ong.Status = "Validada";
            _context.ONGs.Update(ong);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"ONG '{ong.NomeONG}' validada com sucesso!";
            return RedirectToAction("Index");
        }

        // POST: Admin/RejeitarONG/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejeitarONG(string id)
        {
            var ong = await _context.ONGs.FirstOrDefaultAsync(o => o.Id == id);

            if (ong == null)
                return NotFound();

            _context.ONGs.Remove(ong);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"ONG '{ong.NomeONG}' rejeitada e removida!";
            return RedirectToAction("Index");
        }
    }
}
