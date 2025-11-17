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
        public async Task<IActionResult> Index()
        {
            var opportunities = await _context.Opportunities
                .Include(o => o.Organization)
                .ToListAsync();
            return View(opportunities);
        }

        // GET: Opportunities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .Include(o => o.Organization)
                .Include(o => o.VolunteerOpportunities)
                .ThenInclude(vo => vo.Volunteer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (opportunity == null)
            {
                return NotFound();
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
                _context.Add(opportunity);
                await _context.SaveChangesAsync();
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

        // POST: Opportunities/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int opportunityId, string fullName, string email)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Nome e Email são obrigatórios.");
            }

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
                return BadRequest("Este voluntário já está registrado nesta oportunidade.");
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

            return RedirectToAction(nameof(Details), new { id = opportunityId });
        }
    }
}
