using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;
using System.Data;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class EquipamentosController : Controller
    {
        private readonly Database db = new Database();
        public IActionResult Index()
        {
            List<Equipamentos> equipamentos = new List<Equipamentos>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarEquipamentos", conn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    equipamentos.Add(new Equipamentos
                    {
                        nome = reader.GetString("nome"),
                        modelo = reader.GetString("modelo"),
                        foto = reader["foto"] == DBNull.Value ? null : (string?)reader.GetString("foto"),
                        situacao = reader.GetString("situacao"),
                        criado_em = reader.GetDateTime("criado_em")
                    });
                }
            }
            return View(equipamentos);
        }

        public IActionResult Cadastrar()
        {
            using var conn = db.GetConnection();

            ViewBag.Usuarios = CarregarUsuarios(conn);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Equipamentos equipamento)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarEquipamento", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("c_usuario", equipamento.id_usuario);
            cmd.Parameters.AddWithValue("c_nome", equipamento.nome);
            cmd.Parameters.AddWithValue("c_modelo", equipamento.modelo);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
        private List<SelectListItem> CarregarUsuarios(MySqlConnection conn)
        {
            var list = new List<SelectListItem>();
            using var cmd = new MySqlCommand("select id_usuario, nome from Usuarios order by nome", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = rd.GetInt32("id_usuario").ToString(),
                    Text = rd.GetString("nome")
                });
            }
            return list;
        }

    }
}
