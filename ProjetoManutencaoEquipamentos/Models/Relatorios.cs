namespace ProjetoManutencaoEquipamentos.Models
{
    public class Relatorios
    {
        public int id_relatorio { get; set; }
        public int id_ordem { get; set; }
        public int id_tecnico { get; set; }
        public string? assunto { get; set; }
        public string? mensagem { get; set; }
        public DateTime criado_em { get; set; }

        public string? nome_tecnico { get; set; }
    }
}
