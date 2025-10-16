namespace ProjetoManutencaoEquipamentos.Models
{
    public class OrdensServico
    {
        public int? id_ordem { get; set; }
        public int? id_equipamento { get; set; }
        public int? id_tecnico { get; set; }
        public string? titulo { get; set; }
        public string? descricao { get; set; }
        public string? situacao { get; set; }
        public DateTime? criado_em { get; set; }

        public string? nome_tecnico { get; set; }
        public string? modelo_equipamento { get; set; } 

    }
}
