using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class EquipamentosController : Controller
    {
        private readonly Database db = new Database();
        public IActionResult Index()
        {
            List<Equipamentos> equipamentos = new List<Equipamentos>();
            var conn = db.GetConnection();
            var cmd = new MySqlCommand("listarEquipamentos", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {

            }
            return View();
        }
    }
}
