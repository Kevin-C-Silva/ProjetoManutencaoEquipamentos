using Microsoft.AspNetCore.Mvc;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
