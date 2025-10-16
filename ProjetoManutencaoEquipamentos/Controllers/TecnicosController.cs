using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;
using System.Data;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class TecnicosController : Controller
    {
        private readonly Database db = new Database();

        public IActionResult Index()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarTecnicos", conn) { CommandType = CommandType.StoredProcedure })
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(new Usuarios
                    {
                        id_usuario = reader["id_usuario"] == DBNull.Value ? null : (int?)reader.GetInt32("id_usuario"),
                        id_tecnico = reader["id_tecnico"] == DBNull.Value ? null : (int?)reader.GetInt32("id_tecnico"),  // Tratamento para id_tecnico nulo
                        nome = reader.GetString("nome"),
                        email = reader.GetString("email"),
                        role = reader.GetString("role"),
                        criado_em = reader.GetDateTime("criado_em"),
                        especialidade = reader["tecnico_especialidade"] == DBNull.Value ? null : (string?)reader.GetString("tecnico_especialidade"),
                        situacao = reader["tecnico_situacao"] == DBNull.Value ? null : (string?)reader.GetString("tecnico_situacao"),
                    });
                }
            }
            return View(usuarios);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuarios tecnico)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarUsuario", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Separar o CEP no formato XXXXX-XXX
            string cep_inicio = null;
            string cep_fim = null;

            if (!string.IsNullOrEmpty(tecnico.cep) && tecnico.cep.Contains("-"))
            {
                var partes = tecnico.cep.Split('-');
                cep_inicio = partes[0];
                cep_fim = partes[1];
            }

            // Hash da senha
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(tecnico.senha, workFactor: 12);

            // Preenche os parâmetros da procedure
            cmd.Parameters.AddWithValue("c_nome", tecnico.nome);
            cmd.Parameters.AddWithValue("c_espec", tecnico.especialidade ?? "");
            cmd.Parameters.AddWithValue("c_email", tecnico.email);
            cmd.Parameters.AddWithValue("c_senha", senhaHash);
            cmd.Parameters.AddWithValue("c_cep_inicio", cep_inicio ?? "");
            cmd.Parameters.AddWithValue("c_cep_fim", cep_fim ?? "");
            cmd.Parameters.AddWithValue("c_role", "Técnico");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

    }
}
