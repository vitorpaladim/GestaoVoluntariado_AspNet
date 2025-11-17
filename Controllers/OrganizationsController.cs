using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organizations
        public async Task<IActionResult> Index(string searchString)
        {
            var organizations = _context.Organizations.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                organizations = organizations.Where(o =>
                    o.Name.Contains(searchString) ||
                    o.Description.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(await organizations.ToListAsync());
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(organization);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Organização '{organization.Name}' criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Erro ao criar organização: {ex.Message}";
                }
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Organização não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                TempData["ErrorMessage"] = "Organização não encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Organization organization)
        {
            if (id != organization.Id)
            {
                TempData["ErrorMessage"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Organização '{organization.Name}' atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                    {
                        TempData["ErrorMessage"] = "Organização não encontrada.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Erro ao atualizar: {ex.Message}";
                }
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Organização não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var organization = await _context.Organizations
                .Include(o => o.Opportunities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
            {
                TempData["ErrorMessage"] = "Organização não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var organization = await _context.Organizations.FindAsync(id);
                if (organization != null)
                {
                    _context.Organizations.Remove(organization);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Organização '{organization.Name}' deletada com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Organização não encontrada.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao deletar: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organizations.Any(e => e.Id == id);
        }
    }
}