namespace ProjetoManutencaoEquipamentos.Models
{
    public class Usuarios
    {
        public int? id_usuario { get; set; }
        public int? id_tecnico { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? senha { get; set; }
        public string? cep { get; set; }
        public string? role { get; set; }
        public DateTime criado_em { get; set; }

        public string especialidade { get; set; }
        public string situacao { get; set; }
    }
}
