using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;
using GestaoVoluntariado.Models;

namespace GestaoVoluntariado.Controllers
{
    public class OpportunitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpportunitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Opportunities
        public async Task<IActionResult> Index(string searchString, int? organizationId)
        {
            var opportunities = _context.Opportunities
                .Include(o => o.Organization)
                .AsQueryable();

            // Filtro de busca
            if (!string.IsNullOrEmpty(searchString))
            {
                opportunities = opportunities.Where(o =>
                    o.Title.Contains(searchString) ||
                    o.Description.Contains(searchString));
            }

            // Filtro por organização
            if (organizationId.HasValue)
            {
                opportunities = opportunities.Where(o => o.OrganizationId == organizationId.Value);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["OrganizationFilter"] = organizationId;
            ViewData["Organizations"] = new SelectList(
                await _context.Organizations.ToListAsync(),
                "Id",
                "Name"
            );

            return View(await opportunities.OrderBy(o => o.Date).ToListAsync());
        }

        // GET: Opportunities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var opportunity = await _context.Opportunities
                .Include(o => o.Organization)
                .Include(o => o.VolunteerOpportunities)
                .ThenInclude(vo => vo.Volunteer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opportunity == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(opportunity);
        }

        // GET: Opportunities/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OrganizationId"] = new SelectList(
                await _context.Organizations.ToListAsync(),
                "Id",
                "Name"
            );
            return View();
        }

        // POST: Opportunities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Date,OrganizationId")] Opportunity opportunity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(opportunity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Oportunidade '{opportunity.Title}' criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Erro ao criar oportunidade: {ex.Message}";
                }
            }

            ViewData["OrganizationId"] = new SelectList(
                await _context.Organizations.ToListAsync(),
                "Id",
                "Name",
                opportunity.OrganizationId
            );
            return View(opportunity);
        }

        // GET: Opportunities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(
                await _context.Organizations.ToListAsync(),
                "Id",
                "Name",
                opportunity.OrganizationId
            );
            return View(opportunity);
        }

        // POST: Opportunities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,OrganizationId")] Opportunity opportunity)
        {
            if (id != opportunity.Id)
            {
                TempData["ErrorMessage"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Oportunidade '{opportunity.Title}' atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityExists(opportunity.Id))
                    {
                        TempData["ErrorMessage"] = "Oportunidade não encontrada.";
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

            ViewData["OrganizationId"] = new SelectList(
                await _context.Organizations.ToListAsync(),
                "Id",
                "Name",
                opportunity.OrganizationId
            );
            return View(opportunity);
        }

        // GET: Opportunities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var opportunity = await _context.Opportunities
                .Include(o => o.Organization)
                .Include(o => o.VolunteerOpportunities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opportunity == null)
            {
                TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(opportunity);
        }

        // POST: Opportunities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var opportunity = await _context.Opportunities.FindAsync(id);
                if (opportunity != null)
                {
                    _context.Opportunities.Remove(opportunity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Oportunidade '{opportunity.Title}' deletada com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Oportunidade não encontrada.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao deletar: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Opportunities/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int opportunityId, string fullName, string email)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Nome e Email são obrigatórios.";
                return RedirectToAction(nameof(Details), new { id = opportunityId });
            }

            try
            {
                // Check if volunteer already exists
                var volunteer = await _context.Volunteers
                    .FirstOrDefaultAsync(v => v.Email == email);

                if (volunteer == null)
                {
                    // Create new volunteer
                    volunteer = new Volunteer
                    {
                        FullName = fullName,
                        Email = email
                    };
                    _context.Volunteers.Add(volunteer);
                    await _context.SaveChangesAsync();
                }

                // Check if already registered
                var existingRegistration = await _context.VolunteerOpportunities
                    .FirstOrDefaultAsync(vo => vo.VolunteerId == volunteer.Id && vo.OpportunityId == opportunityId);

                if (existingRegistration != null)
                {
                    TempData["ErrorMessage"] = "Você já está inscrito nesta oportunidade.";
                    return RedirectToAction(nameof(Details), new { id = opportunityId });
                }

                // Register volunteer to opportunity
                var volunteerOpportunity = new VolunteerOpportunity
                {
                    VolunteerId = volunteer.Id,
                    OpportunityId = opportunityId,
                    RegisteredAt = DateTime.Now
                };

                _context.VolunteerOpportunities.Add(volunteerOpportunity);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Inscrição realizada com sucesso! Bem-vindo(a), {fullName}!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao realizar inscrição: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = opportunityId });
        }

        // POST: Opportunities/Unregister
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(int opportunityId, int volunteerId)
        {
            try
            {
                var registration = await _context.VolunteerOpportunities
                    .FirstOrDefaultAsync(vo => vo.VolunteerId == volunteerId && vo.OpportunityId == opportunityId);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Inscrição não encontrada.";
                    return RedirectToAction(nameof(Details), new { id = opportunityId });
                }

                _context.VolunteerOpportunities.Remove(registration);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Inscrição cancelada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao cancelar inscrição: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = opportunityId });
        }

        private bool OpportunityExists(int id)
        {
            return _context.Opportunities.Any(e => e.Id == id);
        }
    }
}