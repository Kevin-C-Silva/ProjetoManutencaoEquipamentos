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
        public IActionResult Index(string func) // func -> função (dos usuários que serão pesquisados)
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            using var conn = db.GetConnection();
            using (var cmd = new MySqlCommand("listarUsuarios", conn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("p_role", func);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuarios
                    {
                        id_tecnico = reader.GetInt32("id_tecnico"),
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

        public IActionResult CriarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CriarUsuario(Usuarios usuario)
        {
            using var conn = db.GetConnection();
            using var cmd = new MySqlCommand("cadastrarUsuario", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.senha, workFactor: 12);
            cmd.Parameters.AddWithValue("p_nome", usuario.nome);
            cmd.Parameters.AddWithValue("p_email", usuario.email);
            cmd.Parameters.AddWithValue("p_senha", senhaHash);
            cmd.Parameters.AddWithValue("p_role", usuario.role);
            cmd.Parameters.AddWithValue("cep_inicio", null);
            cmd.Parameters.AddWithValue("cep_fim", null);
            cmd.Parameters.AddWithValue("tecnico_especialidade", usuario.especialidade);
            cmd.Parameters.AddWithValue("tecnico_situacao", usuario.situacao);
            cmd.ExecuteNonQuery();
            return RedirectToAction("CriarUsuario");
        }
    }
}
