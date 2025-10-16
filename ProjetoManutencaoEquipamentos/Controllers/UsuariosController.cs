using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;
using ProjetoManutencaoEquipamentos.Data;
using ProjetoManutencaoEquipamentos.Models;
using System.Data;

namespace ProjetoManutencaoEquipamentos.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Database db = new Database();

        public IActionResult Index()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarUsuarios", conn) { CommandType = CommandType.StoredProcedure })
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
        public IActionResult Cadastrar(Usuarios usuario)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarUsuario", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.senha, workFactor: 12);

            cmd.Parameters.AddWithValue("c_nome", usuario.nome);
            cmd.Parameters.AddWithValue("c_espec", usuario.especialidade ?? "");
            cmd.Parameters.AddWithValue("c_email", usuario.email);
            cmd.Parameters.AddWithValue("c_senha", senhaHash);

            // Tratar cep dividido: supondo que o usuario.cep venha no formato "12345-678"
            string cepInicio = null, cepFim = null;
            if (!string.IsNullOrEmpty(usuario.cep) && usuario.cep.Length == 9)
            {
                cepInicio = usuario.cep.Substring(0, 5);
                cepFim = usuario.cep.Substring(6, 3);
            }

            cmd.Parameters.AddWithValue("c_cep_inicio", cepInicio ?? "");
            cmd.Parameters.AddWithValue("c_cep_fim", cepFim ?? "");
            cmd.Parameters.AddWithValue("c_role", usuario.role);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
    }
}
