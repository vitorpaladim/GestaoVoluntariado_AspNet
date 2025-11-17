using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            var volunteers = await _context.Volunteers
                .Include(v => v.VolunteerOpportunities)
                .ThenInclude(vo => vo.Opportunity)
                .ToListAsync();
            return View(volunteers);
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var volunteer = await _context.Volunteers
                .Include(v => v.VolunteerOpportunities)
                .ThenInclude(vo => vo.Opportunity)
                .ThenInclude(o => o.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (volunteer == null) return NotFound();

            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var volunteer = await _context.Volunteers
                .Include(v => v.VolunteerOpportunities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (volunteer == null) return NotFound();

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}