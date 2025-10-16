using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;
using System.Data;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly Database db = new Database();

        public IActionResult Index()
        {
            List<Relatorios> relatorios = new List<Relatorios>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarRelatorios", conn) { CommandType = CommandType.StoredProcedure })
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    relatorios.Add(new Relatorios
                    {
                        id_relatorio = reader.GetInt32("id_relatorio"),
                        id_ordem = reader.GetInt32("id_ordem"),
                        id_tecnico = reader.GetInt32("id_tecnico"),
                        nome_tecnico = reader.GetString("nome_tecnico"),
                        assunto = reader["assunto"] == DBNull.Value ? null : reader.GetString("assunto"),
                        mensagem = reader["mensagem"] == DBNull.Value ? null : reader.GetString("mensagem"),
                        criado_em = reader.GetDateTime("criado_em"),
                    });
                }
            }
            return View(relatorios);
        }

        public IActionResult Cadastrar()
        {
            using var conn = db.GetConnection();

            ViewBag.Tecnicos = CarregarTecnicos(conn);
            ViewBag.Ordens = CarregarOrdensServico(conn);

            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Relatorios relatorio)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarRelatorio", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("p_ordem", relatorio.id_ordem);
            cmd.Parameters.AddWithValue("p_tecnico", relatorio.id_tecnico);
            cmd.Parameters.AddWithValue("p_assunto", relatorio.assunto);
            cmd.Parameters.AddWithValue("p_msg", relatorio.mensagem);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private List<SelectListItem> CarregarOrdensServico(MySqlConnection conn)
        {
            var list = new List<SelectListItem>();
            using var cmd = new MySqlCommand("select id_ordem, titulo from OrdensServico order by titulo", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = rd.GetInt32("id_ordem").ToString(),
                    Text = rd.GetString("titulo")
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
