using Microsoft.AspNetCore.Mvc;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class TecnicosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
