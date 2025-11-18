using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoVoluntariado.Data;

namespace GestaoVoluntariado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Seu código de ViewBag (vagas populares e causas) permanece aqui...
            var vagasPopulares = await _context.Projetos
                .Include(p => p.ONG)
                .Where(p => p.ONG!.Status == "Validada")
                .OrderByDescending(p => p.Views)
                .Take(6)
                .ToListAsync();

            var causas = new List<string>
            { 
                // ... sua lista de causas ... 
                "Proteção Animal", "Meio Ambiente", "Equidade Racial", "LGBTQIAPN+",
                "Mulheres", "Família Voluntária", "Online", "Pontuais", "RJ"
            };

            ViewBag.VagasPopulares = vagasPopulares;
            ViewBag.Causas = causas;

            // MUDANÇA ESSENCIAL: Forçar o caminho completo (FullPath)
            return View("~/Views/Home/Index.cshtml");
        }
    }
}