using Microsoft.AspNetCore.Mvc;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class RelatoriosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
