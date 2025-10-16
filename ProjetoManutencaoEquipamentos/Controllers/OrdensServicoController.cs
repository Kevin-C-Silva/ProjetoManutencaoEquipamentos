using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;
using System.Data;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class OrdensServicoController : Controller
    {
        private readonly Database db = new Database();

        public IActionResult Index()
        {
            List<OrdensServico> ordens = new List<OrdensServico>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarOrdens", conn) { CommandType = CommandType.StoredProcedure })
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ordens.Add(new OrdensServico
                    {
                        id_ordem = reader["id_ordem"] == DBNull.Value ? null : (int?)reader.GetInt32("id_ordem"),
                        id_equipamento = reader["id_equipamento"] == DBNull.Value ? null : (int?)reader.GetInt32("id_equipamento"),
                        id_tecnico = reader["id_tecnico"] == DBNull.Value ? null : (int?)reader.GetInt32("id_tecnico"),
                        titulo = reader["titulo"] == DBNull.Value ? null : reader.GetString("titulo"),
                        descricao = reader["descricao"] == DBNull.Value ? null : reader.GetString("descricao"),
                        modelo_equipamento = reader["modelo_equipamento"] == DBNull.Value ? null : reader.GetString("modelo_equipamento"),
                        nome_tecnico = reader["nome_tecnico"] == DBNull.Value ? null : reader.GetString("nome_tecnico"),
                        criado_em = reader["criado_em"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime("criado_em"),
                        situacao = reader["situacao"] == DBNull.Value ? null : reader.GetString("situacao"),
                    });
                }
            }
            return View(ordens);
        }

        public IActionResult Cadastrar()
        {
            using var conn = db.GetConnection();

            ViewBag.Equipamentos = CarregarEquipamentos(conn);
            ViewBag.Tecnicos = CarregarTecnicos(conn);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(OrdensServico ordem)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarOrdem", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_equip", ordem.id_equipamento);
            cmd.Parameters.AddWithValue("p_titulo", ordem.titulo);
            cmd.Parameters.AddWithValue("p_desc", ordem.descricao);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private List<SelectListItem> CarregarEquipamentos(MySqlConnection conn)
        {
            var list = new List<SelectListItem>();
            using var cmd = new MySqlCommand("select id_equipamento, nome from Equipamentos order by nome", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = rd.GetInt32("id_equipamento").ToString(),
                    Text = rd.GetString("nome")
                });
            }
            return list;
        }


        private List<SelectListItem> CarregarTecnicos(MySqlConnection conn)
        {
            var list = new List<SelectListItem>();
            using var cmd = new MySqlCommand("select id_tecnico, nome from Tecnicos order by nome", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = rd.GetInt32("id_tecnico").ToString(),
                    Text = rd.GetString("nome")
                });
            }
            return list;
        }
    }
}
