using Microsoft.AspNetCore.Mvc;

namespace GestaoVoluntariado.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
