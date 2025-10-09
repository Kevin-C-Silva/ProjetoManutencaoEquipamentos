using Microsoft.AspNetCore.Mvc;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class EquipamentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
