using Microsoft.AspNetCore.Mvc;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class OrdensServicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
